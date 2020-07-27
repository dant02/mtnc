SET SQL DIALECT 3;

SET NAMES UTF8;

CREATE DATABASE 'd:\fbdata\mtnc.fdb' PAGE_SIZE 16384 DEFAULT CHARACTER SET UTF8;

CREATE TABLE JobRecords (
    id INTEGER NOT NULL,
    text VARCHAR(256) NOT NULL,
    anchorUtc TIMESTAMP NOT NULL,
    duration double precision NOT NULL,
    task VARCHAR(32),
    context VARCHAR(32)
);

CREATE TABLE Activities (
    id INTEGER NOT NULL,
    name VARCHAR(256) NOT NULL,
);



CREATE TABLE People ()

COMMIT;