CREATE TABLE [FOXT].[FORM17TOTALVOLUMEDETAIL]
(
    [form17tvdetailid]          UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM117VOLUMENTDETAIL_form17volumentdetail DEFAULT NEWID(),
    [volumetype]                numeric(1),
    [accumulategasproduction]   numeric(20,4),
    [montlhygasproduction]      numeric(20,4),
    [dailygasproduction]        numeric(20,4),
    [accumulatewaterproduction] numeric(20,4),
    [monthlywaterproduction]    numeric(20,4),
    [dailywaterproduction]      numeric(20,4),
    [formid]                    UNIQUEIDENTIFIER,
    [formation]                 varchar(200)
)
