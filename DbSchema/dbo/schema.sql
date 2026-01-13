CREATE TABLE Articles
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    Title NVARCHAR(250) NOT NULL,

    Type Nvarchar(10) null,

    ShortDescription NVARCHAR(1000) NOT NULL,

    Category NVARCHAR(50) NULL,   -- Tech / Press

    ImagePath NVARCHAR(500) NULL,

    PublishedDate DATETIME2 NOT NULL 
        CONSTRAINT DF_Articles_PublishedDate DEFAULT GETDATE(),

    IsActive BIT NOT NULL 
        CONSTRAINT DF_Articles_IsActive DEFAULT 1
);
ALTER TABLE Articles
ADD
    Slug NVARCHAR(300) NULL,
    Content NVARCHAR(MAX) NULL;


UPDATE Articles
SET Slug = LOWER(
                REPLACE(
                    REPLACE(
                        REPLACE(Title, ' ', '-'),
                    '.', ''),
                ',', '')
           )
WHERE Slug IS NULL;


ALTER TABLE Articles
ADD SortOrder INT NOT NULL CONSTRAINT DF_Articles_SortOrder DEFAULT 0;


select *from Articles

ALTER TABLE Articles
ADD MetaTitle NVARCHAR(160),
    MetaDescription NVARCHAR(300),
    MetaKeywords NVARCHAR(250);

    CREATE TABLE CaseStudies (
    Id INT IDENTITY PRIMARY KEY,
    Title NVARCHAR(250) NOT NULL,
    Slug NVARCHAR(250) NOT NULL UNIQUE,
    ShortDescription NVARCHAR(500),
    ImagePath NVARCHAR(500),
    Category NVARCHAR(100),
    Technology NVARCHAR(200),
    CountryCode NVARCHAR(10),
    IsActive BIT DEFAULT 1,
    SortOrder INT DEFAULT 0,
    CreatedDate DATETIME DEFAULT GETDATE()
);


ALTER TABLE CaseStudies
ADD Technologies NVARCHAR(200);

ALTER TABLE CaseStudies
 CreatedDate DATETIME2 DEFAULT GETDATE();

 ALTER TABLE CaseStudies
ALTER COLUMN CreatedDate datetime2 NOT NULL;

ALTER TABLE CaseStudies
DROP CONSTRAINT DF__CaseStudi__Creat__10E07F16;



CREATE TABLE Leads (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NULL,
    Email NVARCHAR(150)  NULL,
    Phone NVARCHAR(50)  NULL,
    Company NVARCHAR(150) NULL,
    Message NVARCHAR(MAX) NULL,
    NDA BIT  NULL DEFAULT 0,
    Source NVARCHAR(100) NULL,
    PageUrl NVARCHAR(500) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
