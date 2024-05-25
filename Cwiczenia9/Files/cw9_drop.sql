
-- foreign keys
ALTER TABLE Country_Trip DROP CONSTRAINT Country_Trip_Country;

ALTER TABLE Country_Trip DROP CONSTRAINT Country_Trip_Trip;

ALTER TABLE Client_Trip DROP CONSTRAINT Table_5_Client;

ALTER TABLE Client_Trip DROP CONSTRAINT Table_5_Trip;

-- tables
DROP TABLE Client;

DROP TABLE Client_Trip;

DROP TABLE Country;

DROP TABLE Country_Trip;

DROP TABLE Trip;

-- End of file.

