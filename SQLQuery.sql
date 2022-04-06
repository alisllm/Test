Create database Test2;
use Test2
----Создание табличек---

CREATE TABLE Prihod_Deneg
(
	Id int IDENTITY PRIMARY KEY NOT NULL,
	[Data] date NULL,
	Summa money NOT NULL,
	Ostatok MONEY NOT NULL
)
CREATE TABLE Zakaz(
	Id int IDENTITY PRIMARY KEY NOT NULL,
	[Data] date NULL,
	Summa MONEY NOT NULL,
	SummaOplat MONEY NOT NULL
	)
	CREATE TABLE Platej2 (
	Zakaz_id int NOT NULL,
	Prihod_id int NOT NULL,
	SummaPlatej money NOT NULL,
	FOREIGN KEY(Zakaz_id) REFERENCES Zakaz(Id),
	FOREIGN KEY(Prihod_id) REFERENCES Prihod_Deneg(Id)
) 

--Триггер--
CREATE TRIGGER Platej_INSERT
ON platej2
After INSERT
AS 
UPDATE Zakaz
SET SummaOplat = SummaOplat+ SummaPlatej from Platej2 
where Id = (SELECT Zakaz_id FROM inserted)
update Prihod_Deneg
set Ostatok = Ostatok - SummaPlatej from Platej2 
where Id = (SELECT Prihod_id FROM inserted)
--Добавление в бд--
insert into Zakaz(Summa,SummaOplat) values (5025,259), (1000,0),(1000,100),(1500,300)
insert into Prihod_Deneg(Summa,Ostatok) values (5000,2199),(45000,38450),(5000,5000)
