CREATE TABLE Users (
    id            INTEGER     NOT NULL,
    name          VARCHAR(32) NOT NULL,

    createdBy     INTEGER     NOT NULL,
    createdOnUtc  TIMESTAMP   NOT NULL,
    modifiedBy    INTEGER,
    modifiedOnUtc TIMESTAMP
);

CREATE TABLE UserCredentials (
    idPerson      INTEGER     NOT NULL,
    secret        VARCHAR(32) NOT NULL,

    createdBy     INTEGER     NOT NULL,
    createdOnUtc  TIMESTAMP   NOT NULL,
    modifiedBy    INTEGER,
    modifiedOnUtc TIMESTAMP
);