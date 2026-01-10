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

ALTER TABLE Articles
ADD SortOrder INT NOT NULL CONSTRAINT DF_Articles_SortOrder DEFAULT 0;
select *from Articles