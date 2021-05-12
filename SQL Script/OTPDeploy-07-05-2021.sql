--select * from ZbirnikTon order by SortIdx 
--select * from Relacija where Naziv like '%(SLO) 8310 ŠENTJERNEJ - (SLO) 8000 NOVO MESTO%'
--select * from Odpoklic order by 1 desc
--select * from StatusOdpoklica

alter table Odpoklic add ConfirmTS datetime;
alter table Odpoklic add LowestPrice decimal(18,3);


update ZbirnikTon set Koda = 'Ladja-1k', Naziv = 'Ladja-1k' where Koda = 'Ladja'
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-2k', 'Ladja-2k', 24501, 300000, '05.05.2021', 15);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-3k', 'Ladja-3k', 24501, 300000, '05.05.2021', 16);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-4k', 'Ladja-4k', 24501, 300000, '05.05.2021', 17);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-5k', 'Ladja-5k', 24501, 300000, '05.05.2021', 18);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-6k', 'Ladja-6k', 24501, 300000, '05.05.2021', 19);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-7k', 'Ladja-7k', 24501, 300000, '05.05.2021', 20);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-8k', 'Ladja-8k', 24501, 300000, '05.05.2021', 21);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-9k', 'Ladja-9k', 24501, 300000, '05.05.2021', 22);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-10k', 'Ladja-10k', 24501, 300000, '05.05.2021', 23);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-11k', 'Ladja-11k', 24501, 300000, '05.05.2021', 24);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-12k', 'Ladja-12k', 24501, 300000, '05.05.2021', 25);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-13k', 'Ladja-13k', 24501, 300000, '05.05.2021', 26);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-14k', 'Ladja-14k', 24501, 300000, '05.05.2021', 27);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-15k', 'Ladja-15k', 24501, 300000, '05.05.2021', 28);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-16k', 'Ladja-16k', 24501, 300000, '05.05.2021', 29);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-17k', 'Ladja-17k', 24501, 300000, '05.05.2021', 30);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-18k', 'Ladja-18k', 24501, 300000, '05.05.2021', 31);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-19k', 'Ladja-19k', 24501, 300000, '05.05.2021', 32);
insert ZbirnikTon(Koda, Naziv, OdTeza, DoTeza, ts, SortIdx) values ('Ladja-20k', 'Ladja-20k', 24501, 300000, '05.05.2021', 33);

update ZbirnikTon set SortIdx = 34 where Koda = 'Vlak'

update Relacija set Naziv='1x (SLO) 8310 ŠENTJERNEJ - (SLO) 8000 NOVO MESTO' where Naziv = '(SLO) 8310 ŠENTJERNEJ - (SLO) 8000 NOVO MESTO'

INSERT INTO Relacija(Naziv, Dolzina, Datum, ts, tsIDOsebe)  values ('2x (SLO) 8310 ŠENTJERNEJ - (SLO) 8000 NOVO MESTO',0,'06-05-2021','06-05-2021',1);
INSERT INTO Relacija(Naziv, Dolzina, Datum, ts, tsIDOsebe)  values ('3x (SLO) 8310 ŠENTJERNEJ - (SLO) 8000 NOVO MESTO',0,'06-05-2021','06-05-2021',1);
INSERT INTO Relacija(Naziv, Dolzina, Datum, ts, tsIDOsebe)  values ('4x (SLO) 8310 ŠENTJERNEJ - (SLO) 8000 NOVO MESTO',0,'06-05-2021','06-05-2021',1);
INSERT INTO Relacija(Naziv, Dolzina, Datum, ts, tsIDOsebe)  values ('5x (SLO) 8310 ŠENTJERNEJ - (SLO) 8000 NOVO MESTO',0,'06-05-2021','06-05-2021',1);
INSERT INTO Relacija(Naziv, Dolzina, Datum, ts, tsIDOsebe)  values ('6x (SLO) 8310 ŠENTJERNEJ - (SLO) 8000 NOVO MESTO',0,'06-05-2021','06-05-2021',1);
INSERT INTO Relacija(Naziv, Dolzina, Datum, ts, tsIDOsebe)  values ('7x (SLO) 8310 ŠENTJERNEJ - (SLO) 8000 NOVO MESTO',0,'06-05-2021','06-05-2021',1);
INSERT INTO Relacija(Naziv, Dolzina, Datum, ts, tsIDOsebe)  values ('8x (SLO) 8310 ŠENTJERNEJ - (SLO) 8000 NOVO MESTO',0,'06-05-2021','06-05-2021',1);
INSERT INTO Relacija(Naziv, Dolzina, Datum, ts, tsIDOsebe)  values ('9x (SLO) 8310 ŠENTJERNEJ - (SLO) 8000 NOVO MESTO',0,'06-05-2021','06-05-2021',1);
INSERT INTO Relacija(Naziv, Dolzina, Datum, ts, tsIDOsebe)  values ('10x (SLO) 8310 ŠENTJERNEJ - (SLO) 8000 NOVO MESTO',0,'06-05-2021','06-05-2021',1);


