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
CREATE TRIGGER Platej_INSERT2
ON platej2
After INSERT
AS 
declare @ostatokS money;
declare @summS money;
declare @summaOplat money;
select @ostatokS = ostatok FROM Prihod_Deneg WHERE Id = (SELECT Prihod_id FROM inserted);
select @summS = Summa FROM Zakaz WHERE Id = (SELECT Zakaz_id FROM inserted);
select @summaOplat = SummaOplat FROM Zakaz WHERE Id = (SELECT Zakaz_id FROM inserted);
---Равно
if ((@summS-@summaOplat) = @ostatokS)
begin
update Platej2
set SummaPlatej = @ostatokS where SummaPlatej =0
update Zakaz
set SummaOplat = SummaOplat + @ostatokS where id = (SELECT Zakaz_id FROM inserted);
update Prihod_Deneg
set Ostatok = 0 where Id = (SELECT Prihod_id FROM inserted);
end;
---БОЛЬШЕ
if ((@summS-@summaOplat) < @ostatokS)
begin
update Platej2
set SummaPlatej = (@summS-@summaOplat) where SummaPlatej =0
update Zakaz
set SummaOplat = Summa where id = (SELECT Zakaz_id FROM inserted);
update Prihod_Deneg
set Ostatok = (@ostatokS - (@summS-@summaOplat)) where Id = (SELECT Prihod_id FROM inserted);
end;
---МЕНЬШЕ
if ((@summS-@summaOplat) > @ostatokS)
begin
update Platej2
set SummaPlatej = @ostatokS where SummaPlatej =0
update Zakaz
set SummaOplat = @summaOplat+@ostatokS where id = (SELECT Zakaz_id FROM inserted);
update Prihod_Deneg
set Ostatok = 0 where Id = (SELECT Prihod_id FROM inserted);
end;

--Добавление в бд--
insert into Zakaz(Summa,SummaOplat) values (5025,259), (1000,0),(1000,100),(1500,300)
insert into Prihod_Deneg(Summa,Ostatok) values (5000,2199),(45000,38450),(5000,5000)
