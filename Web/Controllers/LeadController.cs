using Data;
using Google;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

[ApiController]
[Route("api/lead")]
public class LeadController : ControllerBase
{
    private readonly EmailService _emailService;
    private readonly AppDbContext _db;

    public LeadController(EmailService emailService, AppDbContext db)
    {
        _emailService = emailService;
        _db= db;    
    }

    [HttpPost]
    public IActionResult SubmitLead([FromBody] LeadRequest lead)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data");


        var leadEntity = new Lead
        {
            FullName = lead.FullName,
            Email = lead.Email,
            Phone = lead.Phone,
            Company = lead.Company,
            Message = lead.Message,
            NDA = lead.NDA,
            Source = lead.Source,
            PageUrl = lead.PageUrl
        };

        _db.Leads.Add(leadEntity);
        _db.SaveChanges();

        // 🔔 ADMIN EMAIL
        var adminBody = $@"
            <h2>New Consultation Request</h2>
            <p><strong>Name:</strong> {lead.FullName}</p>
            <p><strong>Email:</strong> {lead.Email}</p>
            <p><strong>Phone:</strong> {lead.Phone}</p>
            <p><strong>Company:</strong> {lead.Company}</p>
            <p><strong>NDA:</strong> {(lead.NDA ? "Yes" : "No")}</p>
            <p><strong>Message:</strong><br/>{lead.Message}</p>
            <p><strong>Source:</strong> {lead.Source}</p>
            <p><strong>Page:</strong> {lead.PageUrl}</p>
        ";

        _emailService.Send(
            to: "info@cohostweb.com",
            subject: "New Free Consultation Lead",
            body: adminBody
        );

        // ✅ THANK YOU EMAIL TO USER
        SendThankYouEmail(lead);

        return Ok(new { success = true });
    }

    private void SendThankYouEmail(LeadRequest lead)
    {
        var body = $@"
            <p>Hi {lead.FullName},</p>

            <p>Thank you for reaching out to <strong>Cohost Web</strong>.</p>

            <p>We’ve received your request for a <strong>free 30-minute strategy consultation</strong>.
            One of our senior consultants will contact you within <strong>24 hours</strong>.</p>

            <p><strong>What happens next?</strong></p>
            <ul>
              <li>We review your requirements</li>
              <li>Prepare technical insights & recommendations</li>
              <li>Discuss timelines, architecture & cost estimates</li>
            </ul>

            <p>If you’d like to talk sooner, you can:</p>
            <ul>
              <li>📞 Call us: +91 9024255861</li>
              <li>💬 WhatsApp us anytime</li>
              <li>📅 Schedule a call via our website</li>
            </ul>

            <p>Best regards,<br/>
            <strong>Cohost Web Team</strong><br/>
            https://www.cohostweb.com</p>
        ";

        _emailService.Send(
            to: lead.Email,
            subject: "We’ve Received Your Consultation Request",
            body: body
        );
    }
}
