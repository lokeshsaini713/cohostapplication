  //  document.getElementById("user-form").addEventListener("submit", function (e) {
  //      e.preventDefault();

  //  // Clear previous errors
  //  document.querySelectorAll(".form-error").forEach(el => el.remove());
  //  document.querySelectorAll(".is-invalid").forEach(el => el.classList.remove("is-invalid"));

  //  let isValid = true;

  //  const fullName = document.getElementById("fullName");
  //  const email = document.getElementById("email");
  //  const phone = document.getElementById("phoneNumber");

  //  // Name validation
  //  if (fullName.value.trim().length < 2) {
  //      showError(fullName, "Please enter your full name");
  //  isValid = false;
  //  }

  //  // Email validation
  //  if (!/^\S+@\S+\.\S+$/.test(email.value)) {
  //      showError(email, "Please enter a valid email address");
  //  isValid = false;
  //  }

  //  // Phone validation
  //  if (!/^[0-9+\-\s]{8, 15}$/.test(phone.value)) {
  //      showError(phone, "Please enter a valid phone number");
  //  isValid = false;
  //  }

  //  if (!isValid) return;

  //  // Simulate successful submit (replace with API call)
  //  document.getElementById("formSuccess").classList.remove("d-none");
  //  this.reset();

  //  // Optional: scroll to success message
  //  document.getElementById("formSuccess").scrollIntoView({behavior: "smooth" });
//});

function sendwhatapp() {
    const whatsappLink =
        "https://wa.me/919024255861?text=" +
        encodeURIComponent(
            `Hi, I'm ${formData.fullName}.
Email: ${formData.email}
Project: ${formData.message}`
        );
}

    function showError(input, message) {
        input.classList.add("is-invalid");
    const error = document.createElement("div");
    error.className = "form-error";
    error.innerText = message;
    input.parentElement.appendChild(error);
  }


    document.getElementById("user-form").addEventListener("submit", async function (e) {
        e.preventDefault();
        debugger;
    clearErrors();
    let isValid = validateForm();
    if (!isValid) return;

    const formData = {
        fullName: fullName.value.trim(),
    email: email.value.trim(),
    phone: phoneNumber.value.trim(),
    company: document.getElementById("companyName").value.trim(),
    message: document.getElementById("message").value.trim(),
    nda: document.getElementById("nda").checked,
    source: "Contact Page",
    pageUrl: window.location.href
    };

    try {
        const response = await fetch("/api/lead", {
        method: "POST",
    headers: {
        "Content-Type": "application/json"
        },
    body: JSON.stringify(formData)
      });

        if (!response.ok) throw new Error("Submission failed");

    showSuccess();
    this.reset();

    } catch (error) {
        alert("Something went wrong. Please try again or contact us directly.");
    }
  });

    function showSuccess() {
    //    document.getElementById("formSuccess").classList.remove("d-none");
        //document.getElementById("formSuccess").scrollIntoView({behavior: "smooth" });
            // prevent duplicate messages
            if (document.getElementById("formSuccess")) return;

            const successDiv = document.createElement("div");
            successDiv.id = "formSuccess";
            successDiv.className = "alert alert-success mt-3";

            successDiv.innerHTML = `
        ✅ <strong>Thank you!</strong> Your request has been submitted successfully.<br>
        Our team will contact you within 24 hours.
    `;

            // insert after the form
            const form = document.getElementById("user-form");
            form.after(successDiv);

            successDiv.scrollIntoView({ behavior: "smooth" });

            // auto-hide after 5 seconds (optional)
            setTimeout(() => {
                successDiv.remove();
            }, 5000);
        }

    function clearErrors() {
        document.querySelectorAll(".form-error").forEach(e => e.remove());
    document.querySelectorAll(".is-invalid").forEach(e => e.classList.remove("is-invalid"));
  }

    function validateForm() {
        let valid = true;

    if (fullName.value.trim().length < 2) {
        showError(fullName, "Please enter your full name");
    valid = false;
    }

    if (!/^\S+@\S+\.\S+$/.test(email.value)) {
        showError(email, "Enter a valid email address");
    valid = false;
    }

        if (!/^\+?\d{10,13}$/.test(phoneNumber.value)) {
        showError(phoneNumber, "The phone number must be 10 to 13 characters long and contain only digits, spaces, + or -.");
    valid = false;
    }

    return valid;
  }

    function showError(input, message) {
        input.classList.add("is-invalid");
    const error = document.createElement("div");
    error.className = "form-error";
    error.innerText = message;
    input.parentElement.appendChild(error);
  }
