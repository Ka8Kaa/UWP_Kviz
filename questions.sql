CREATE TABLE Questions (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    QuestionText TEXT NOT NULL,
    CorrectAnswer TEXT NOT NULL,
    WrongAnswer1 TEXT NOT NULL,
    WrongAnswer2 TEXT NOT NULL,
    WrongAnswer3 TEXT NOT NULL
);

INSERT INTO Questions (QuestionText, CorrectAnswer, WrongAnswer1, WrongAnswer2, WrongAnswer3)
VALUES ('Koji je glavni grad Francuske?', 'Pariz', 'London', 'Berlin', 'Rim');

INSERT INTO Questions (QuestionText, CorrectAnswer, WrongAnswer1, WrongAnswer2, WrongAnswer3)
VALUES ('Koja je najveća životinja na svijetu?', 'Plavi kit', 'Slon', 'Gorila', 'Tigar');

INSERT INTO Questions (QuestionText, CorrectAnswer, WrongAnswer1, WrongAnswer2, WrongAnswer3)
VALUES ('Što dolazi nakon koncerta?', 'Bis', 'Fis', 'Cis', 'Gis');

INSERT INTO Questions (QuestionText, CorrectAnswer, WrongAnswer1, WrongAnswer2, WrongAnswer3)
VALUES ('Koji je auto 1991. u Osjeku stradao pod gusjenicama tenka?', 'Fićo', 'Peglica', 'Buba', 'Spaček');