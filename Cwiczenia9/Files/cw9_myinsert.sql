-- Insert into Client
INSERT INTO Client (IdClient, FirstName, LastName, Email, Telephone, Pesel)
VALUES (1, 'John', 'Doe', 'john.doe@example.com', '123456789', '12345678901'),
       (2, 'Jane', 'Smith', 'jane.smith@example.com', '987654321', '09876543210');

-- Insert into Country
INSERT INTO Country (IdCountry, Name)
VALUES (1, 'Poland'),
       (2, 'Germany');

-- Insert into Trip
INSERT INTO Trip (IdTrip, Name, Description, DateFrom, DateTo, MaxPeople)
VALUES (1, 'Trip to Warsaw', 'A wonderful trip to the capital of Poland.', '2023-06-01', '2023-06-10', 20),
       (2, 'Trip to Berlin', 'Experience the history and culture of Berlin.', '2023-07-15', '2023-07-20', 15);

-- Insert into Client_Trip
INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate)
VALUES (1, 1, '2023-05-01', '2023-05-10'),
       (2, 2, '2023-06-01', NULL);

-- Insert into Country_Trip
INSERT INTO Country_Trip (IdCountry, IdTrip)
VALUES (1, 1), -- Poland -> Trip to Warsaw
       (2, 2); -- Germany -> Trip to Berlin