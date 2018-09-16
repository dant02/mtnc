SET SQL DIALECT 3;

SET NAMES UTF8;

CREATE DATABASE 'd:\mtnc.fdb' USER 'SYSDBA' PASSWORD 'masterkey' PAGE_SIZE 16384 DEFAULT CHARACTER SET UTF8;

CREATE TABLE Calendars (
    id            INTEGER     NOT NULL,
    name          VARCHAR(32) NOT NULL,

    createdBy     INTEGER     NOT NULL,
    createdOnUtc  TIMESTAMP   NOT NULL,
    modifiedBy    INTEGER,
    modifiedOnUtc TIMESTAMP
);

CREATE TABLE CalendarSubscribers (
    idCalendar   INTEGER   NOT NULL,
    idPerson     INTEGER   NOT NULL,

    createdBy    INTEGER   NOT NULL,
    createdOnUtc TIMESTAMP NOT NULL
);

CREATE TABLE CalendarUnits (
    id                   INTEGER     NOT NULL,
    name                 VARCHAR(32) NOT NULL,
    idParentCalendarUnit INTEGER,
    quantityInParent     INTEGER,

    createdBy            INTEGER     NOT NULL,
    createdOnUtc         TIMESTAMP   NOT NULL,
    modifiedBy           INTEGER,
    modifiedOnUtc        TIMESTAMP
);


CREATE TABLE CalendarItems (
    id                   INTEGER     NOT NULL,
    name                 VARCHAR(32) NOT NULL,
    idCalendar           INTEGER     NOT NULL,
    idCalendarUnit       INTEGER,
    calendarUnitQuantity INTEGER,
    anchorUtc            TIMESTAMP,

    createdBy            INTEGER     NOT NULL,
    createdOnUtc         TIMESTAMP   NOT NULL,
    modifiedBy           INTEGER,
    modifiedOnUtc        TIMESTAMP
);
