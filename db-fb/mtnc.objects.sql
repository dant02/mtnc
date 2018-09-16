CREATE TABLE CalendarItemObject (
    idCalendarItem    INTEGER NOT NULL,
    idCalendarContact INTEGER NOT NULL,
);

-- object of interest
CREATE TABLE CalendarObject (
    id                     INTEGER NOT NULL,
    idParentCalendarObject INTEGER,
    idCalendarObjectType   INTEGER,
    value                  VARCHAR(32) NOT NULL,
);

CREATE TABLE CalendarObjectTypes (
    id   INTEGER     NOT NULL,
    name VARCHAR(32) NOT NULL
);