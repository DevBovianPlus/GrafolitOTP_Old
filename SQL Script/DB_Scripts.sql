/*GRAFOLIT OTP - skripte*/

/*
	-- Dodajanje novega stolpca RazpisPozicijaID na tabelo Odpoklic
	-- Spreminjanje imena vlog in dodajanje nove vloge Leader
	-- Spreminjanje imena SystemEmailMessage v SystemEmailMessage_OTP
	-- Spreminjanje imena SystemMessageEvents v SystemMessageEvents_OTP

	-- 30.1.2019
	-- dodajanje Polja MaterialIdent

	--6.2.2019
	--dodajanje polja na OdpoklicPozicija
		--DatumVnosa
		--KolicinaOTP
		--StatusPrevzeto

	--12.2.2019
	--dodajanje polja na Odpoklic in OdpoklicPozicija
		--OdpoklicStevilka
		--ZaporednaStevilka
	-- dodajanje novih statusev za odpoklic (Prevzet, delno prevzet)

	--20.2.2019
	--Dodajanje polja KolicinaOTPPozicijaNarocilnice na OdpoklicPozicija

	--22.2.2019
	--Dodajanje polja in reference UserID, DatumNaklada

	--25.2.2019
	--Dodajanje polj na Odpoklic in OdpoklicPozicija tabela
		--Dobavitelj(kraj, posta, naslov, naziv)
		--Kupec (kraj, posta, naslov)

	--1.3.2019
	-- Dodajanje polja Proizvedeno

	--27.3.2019
	-- Dodajanje polja RazlogOdobritveSistem v tabelo Odpoklic
*/

alter table OdpoklicPozicija
add DatumVnosa datetime,
KolicinaOTP decimal(18,3),
StatusPrevzeto bit

alter table Odpoklic
add OdpoklicStevilka int

alter table OdpoklicPozicija
add ZaporednaStevilka int

insert into StatusOdpoklica
values ('PREVZET','Prevzet','Odpoklic prevzet v celoti',NULL,GETDATE()),
('DELNO_PREVZET','Delno prevzet','Odpoklic delno prevzet',NULL,GETDATE())


alter table OdpoklicPozicija
add KolicinaOTPPozicijaNarocilnice decimal(18,3)

alter table Odpoklic
add UserID int,
DatumNaklada datetime,
constraint FK_UserID foreign key(UserID) references Osebe_OTP(idOsebe)

alter table Odpoklic
add DobaviteljNaziv varchar(600),
DobaviteljNaslov varchar(500),
DobaviteljPosta varchar(100),
DobaviteljKraj varchar(200)

alter table OdpoklicPozicija
add KupecNaslov varchar(500),
KupecPosta varchar(100),
KupecKraj varchar(200)

/* 1.3.2019 */
alter table OdpoklicPozicija
add Proizvedeno decimal(18,3)

/* 4.3.2019 */
alter table Odpoklic
alter column DobaviteljID int null

alter table Odpoklic
add DobaviteljUrediTransport bit

/* 19.03.2019 */
INSERT INTO Stranka_OTP (KodaStranke, TipID, NazivPrvi, NazivDrugi, ts, tsIDOsebe) values ('DOB', 2, 'Dobavitelj','Dobavitelj', CURRENT_TIMESTAMP, 1);


/*27.3.2019*/
--alter table Odpoklic
--add RazlogOdobritveSistem varchar(150)


/*VIEW*/
SELECT	G.adDate AS Datum_Prevzema,
		G.acIssuer AS Dobavitelj,
		G.acDoc2 AS Stevilka_Odpoklica,
		P.acIdent AS Ident,
		P.acName AS Artikel,
		P.anQty AS Kolicina_Prevzema
FROM Grafolit55SI.dbo.tHE_Move AS G 
INNER JOIN Grafolit55SI.dbo.tHE_MoveItem AS P ON G.acKey = P.acKey
WHERE	LEFT(G.acDocType, 3) IN ('170')
		AND G.acDoc2 <> ''
		AND CAST(G.adDate AS date) = CAST(GETDATE() AS date)


/*8.4.2019*/
--alter table Odpoklic
--alter column RelacijaID int null;
--alter table Odpoklic
--alter column CenaPrevoza decimal(18,3) null;


--create table TipPrevoza (
--	TipPrevozaID int not null identity(1,1) primary key,
--	Koda varchar(20) not null,
--	Naziv varchar(200) not null,
--	Opombe varchar(2000) null,
--	DovoljenaTeza decimal(18,0) null,
--	ShranjevanjePozicij bit null,
--	ts datetime null,
--	tsIDPrijave int null,
--);

--alter table Stranka_OTP
--add TipPrevoza int null,
--constraint FK_TipPrevoza foreign key(TipPrevoza) references TipPrevoza(TipPrevozaID);

/*10.4.2019*/
alter table Odpoklic
add LastenPrevoz bit null;

insert into TipStranka
values ('SKLADISCE', 'Skladišèe', 'Tip stranke skladišèe', NULL,'10-04-2019');

/*11.4.2019*/
insert into TipPrevoza
values ('LADJA', 'Ladja', 'Ladijski prevoz', 300000, 0,GETDATE(),0),
 ('LETALO', 'Letalo', 'Letalski prevoz', 0, 0,GETDATE(),0),
 ('KAMION', 'Kamion', 'Prevoz z kamionom', 24500, 0,GETDATE(),0),
 ('KOMBI', 'Kombi', 'Prevoz z kombijem', 0, 0,GETDATE(),0),
 ('ZBIRNIK', 'Zbirnik', '', 0, 0,GETDATE(),0);

 alter table Odpoklic
add TipPrevoza int null,
LastnoSkladisceID int null,
constraint FK_TipPrevoza_Stranka foreign key(TipPrevoza) references TipPrevoza(TipPrevozaID),
constraint FK_LastnoSkladisce_Stranka foreign key(LastnoSkladisceID) references Stranka_OTP(idStranka);

/*15.4.2019*/
alter table OdpoklicPozicija
add OdpoklicIzLastneZaloge bit null

/*16.4.2019*/
insert into StatusOdpoklica
values ('PONOVNI_ODPOKLIC', 'Ponovni odpoklic', 'Odpoklici ki so bili izbrani za ponovno odpoklic na neprevzetih pozicijah', NULL, GETDATE());

alter table OdpoklicPozicija
add PrvotniOdpoklicPozicijaID int null;

/*19.4.2019*/
alter table OdpoklicPozicija
add Split bit null;

alter table OdpoklicPozicija
add EnotaMere varchar(150) null,
TransportnaKolicina decimal(18,3);


USE [GrafolitOTP_Prod]
GO
/****** Object: Table [dbo].[OdpoklicPozicija] Script Date: 5. 04. 2019 11:12:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LastnaZaloga](
[idLastnaZaloga] [int] IDENTITY(1,1) NOT NULL,
[LastnoSkladisceID] [int] NOT NULL,
[OdpoklicID] [int] NOT NULL,
[NarociloID] [varchar](200) NOT NULL,
[NarociloPozicijaID] [int] NOT NULL,
[TipID] [int] NOT NULL,
[Kolicina] [decimal](18, 3) NOT NULL,
[KolicinaIzNarocila] [decimal](18, 3) NOT NULL,
[StatusKolicine] [int] NOT NULL,
[Material] [varchar](500) NULL,
[tsIDOseba] [int] NULL,
[ts] [datetime] NULL CONSTRAINT [DF_LastnaZaloga_ts] DEFAULT (getdate()),
[OC] [varchar](300) NULL,
[KolicinaPrevzeta] [decimal](18, 3) NULL,
[KolicinaRazlika] [decimal](18, 3) NULL,
[Palete] [int] NULL,
[KupecNaziv] [varchar](300) NULL,
[KupecViden] [int] NULL,
[TrenutnaZaloga] [decimal](18, 3) NULL,
[OptimalnaZaloga] [decimal](18, 3) NULL,
[TipNaziv] [varchar](250) NULL,
[Interno] [varchar](200) NULL,
[Proizvedeno] [decimal](18, 3) NULL,
[MaterialIdent] [varchar](300) NULL,
[KolicinaOTPPozicijaNarocilnice] [decimal](18, 3) NULL,
[ZaporednaStevilka] [int] NULL,
[DatumVnosa] [datetime] NULL,
[KolicinaOTP] [decimal](18, 3) NULL,
[StatusPrevzeto] [bit] NULL,
[KupecNaslov] [varchar](500) NULL,
[KupecPosta] [varchar](100) NULL,
[KupecKraj] [varchar](200) NULL,
CONSTRAINT [PK_LastnaZaloga] PRIMARY KEY CLUSTERED 
(
[idLastnaZaloga] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[LastnaZaloga] WITH CHECK ADD CONSTRAINT [FK_LastnaZaloga_Odpoklic] FOREIGN KEY([OdpoklicID])
REFERENCES [dbo].[Odpoklic] ([OdpoklicID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LastnaZaloga] WITH CHECK ADD CONSTRAINT [FK_LastnaZaloga_Stranka_OTP] FOREIGN KEY([LastnoSkladisceID])
REFERENCES [dbo].[Stranka_OTP] ([idStranka])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LastnaZaloga] CHECK CONSTRAINT [FK_LastnaZaloga_Odpoklic]


alter table LastnaZaloga
add EnotaMere varchar(150) null,
TransportnaKolicina decimal(18,3);


/*21.6.2019*/
alter table RazpisPozicija
add PrevoznikAktualnaCena bit;

/*2.8.2019*/
alter table Odpoklic
add Prevozniki varchar(1000),
KupecUrediTransport bit;

/* 21.08.2019 Funkcija, ki vrne Število odpoklicev v zadnjem letu */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Danijel Novak
-- Create date: 20.08.2019
-- Description:	Funkcija vrne število odpoklicev po relaciji in po relaciji in dobavitelju
-- =============================================
ALTER FUNCTION GetLastYearRecallCountBySuplierAndRoute
(	
	-- Add the parameters for the function here
	@RelacijaID int,
	@DobaviteljID int
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	WITH Rel_Dob AS
	(
	SELECT RelacijaID, DobaviteljID, COUNT(*) AS St_Odpoklicev_Dobavitelj
	FROM Odpoklic
	WHERE	(StatusID<>3 and StatusID<>1)
			AND RelacijaID = @RelacijaID
			AND DobaviteljID = @DobaviteljID
			AND DATEDIFF(DAY, ts, GETDATE()) <= 365 
	GROUP BY RelacijaID, DobaviteljID
	),

	Rel AS
	(
	SELECT RelacijaID, COUNT(*) AS St_Odpoklicev_Relacija
	FROM Odpoklic
	WHERE	(StatusID<>3 and StatusID<>1)
			AND RelacijaID = @RelacijaID
			AND DATEDIFF(DAY, ts, GETDATE()) <= 365 
	GROUP BY RelacijaID
	)

	SELECT RD.RelacijaID, RD.DobaviteljID,
			CASE 
				WHEN DATEDIFF(DAY, '20190301', CAST(GETDATE() AS date)) < 365
					THEN RD.St_Odpoklicev_Dobavitelj * 365 / DATEDIFF(DAY, '20190301', CAST(GETDATE() AS date))
				ELSE RD.St_Odpoklicev_Dobavitelj	
			END AS St_Relacija_Dobavitelj,
			CASE 
				WHEN DATEDIFF(DAY, CAST(GETDATE() AS date), '20190301') < 365
					THEN R.St_Odpoklicev_Relacija * 365 / DATEDIFF(DAY, '20190301', CAST(GETDATE() AS date))
				ELSE R.St_Odpoklicev_Relacija	
			END AS St_Relacija
	FROM Rel_Dob AS RD
		JOIN Rel AS R
			ON RD.RelacijaID = R.RelacijaID
)
GO


/*20.8.2019*/
alter table Odpoklic alter column Opis varchar(5000);
alter table Odpoklic alter column OdobritevKomentar varchar(5000);

/*22.8.2019*/
alter table Razpis 
add RazpisKreiran bit;

update Razpis
set RazpisKreiran = 1;

alter table Stranka_OTP
add TipPrevoza int null,
constraint FK_TipPrevoza foreign key(TipPrevoza) references TipPrevoza(TipPrevozaID);

create table PrijavaPrevoznika (
	PrijavaPrevoznikaID int not null identity(1,1) primary key,
	OdpoklicID int not null,
	PrevoznikID int not null,
	PrvotnaCena decimal(18,3) not null,
	PrijavljenaCena decimal(18,3) null,
	DatumNaklada datetime not null,
	DatumPosiljanjePrijav datetime not null,
	DatumPrijave datetime null,
	ts datetime null,

	constraint FK_OdpoklicID foreign key(OdpoklicID) references Odpoklic(OdpoklicID),
	constraint FK_PrevoznikID foreign key(PrevoznikID) references Stranka_OTP(idStranka)
);

alter table Odpoklic
add PovprasevanjePoslanoPrevoznikom bit,
PrevoznikOddalNajnizjoCeno bit

insert into StatusOdpoklica
values ('RAZPIS_PREVOZNIK', 'Razpis za prevoznika', 'Razpis za oddajo cene za prevoznika', NULL,'09-16-2019'),
('POTRJEN_PREVOZNIK', 'Prevoznik potrjen', '', NULL,'09-16-2019');

alter table Odpoklic
add DatumPosiljanjaMailLogistika datetime

insert into Vloga_OTP
values('Logistics', 'Oddelek logistika', GETDATE(), NULL)

select * from Vloga_OTP

select * from Stranka_OTP

select * from SystemEmailMessage_OTP order by 1 desc

update SystemEmailMessage_OTP 
set Status = 1
where SystemEmailMessageID in (169,172,173,175,176)


select * from PrijavaPrevoznika

select * from StatusOdpoklica

select * from Odpoklic where PovprasevanjePoslanoPrevoznikom = 1

update Odpoklic
set PrevoznikOddalNajnizjoCeno=0, DatumPosiljanjaMailLogistika = NULL
where OdpoklicID in (1316,1317)


select * from PrijavaPrevoznika

update PrijavaPrevoznika
set PrijavljenaCena = 90, DatumPrijave = '2019-09-25'
where PrijavaPrevoznikaID = 12

select * from SystemEmailMessage_OTP order by 1 desc

update Stranka_OTP
set Email='danijel.novak@sqlconsult.si'

select * from Stranka_OTP

select * from Osebe_OTP
select * from Vloga_OTP

insert into Osebe_OTP
values(6, 'Logistika',NULL,NULL,NULL,'polegekmartin@gmail.com',NULL,'Logistika','Logistika',NULL,NULL,NULL,NULL, GETDATE(), NULL)

update Osebe_OTP
set Email = 'danijel.novak@sqlconsult.si'
where idVloga = 6

select * from Odpoklic ORDER BY 1 desc

select * from Stranka_OTP

select * from PrijavaPrevoznika

delete from PrijavaPrevoznika
where PrijavaPrevoznikaID in (4,5,6,7,8,9,10);

select * from Stranka_OTP where idStranka = 4

select * from RazpisPozicija where Cena > 0 and StrankaID = 12 and RelacijaID = 6 order by Cena asc, ts desc

select * from RazpisPozicija where Cena > 0 and RelacijaID = 6


select * from SystemEMailMessage_OTP order by 1 desc

use GrafolitCRM
select * from SystemEmailMessage order by 1 desc

-- 29.10.2019 - Polje za opombo pri prijavi prevoznika
alter table Odpoklic add OpombaZaPovprasevnjePrevoznikom varchar(4000);

-- 15.11.2019 - Polja za pantheon klice in preverjanja
alter table Odpoklic add P_CreateOrder datetime, P_UnsuccCountCreatePDFPantheon int, P_LastTSCreatePDFPantheon datetime, P_TransportOrderPDFName varchar(50), P_TransportOrderPDFDocPath varchar(400), P_GetPDFOrderFile datetime
INSERT INTO StatusOdpoklica values('USTVARJENO_NAROCILO', 'Ustvarjeno naroèilo v Pantheon','Naroèilo je bilo uspešno ustvarjeno v Pantheon-u', 1,CURRENT_TIMESTAMP); 
INSERT INTO StatusOdpoklica values('KREIRAN_PDF', 'Pantheon je kreiral PDF','Pantheon je kreiral PDF,ki se ga pošlje prevozniku', 1,CURRENT_TIMESTAMP); 
alter table SystemEmailMessage_OTP add Attachments varchar(4000);
alter table PrijavaPrevoznika add OpombaPrijave varchar(4000);

-- 09.12.2019 - Dodati polja za Aktivnost stranke in Datum Razklada na opoklicu
alter table Stranka_OTP add Activity int;
update Stranka_OTP set Activity = 1;
alter table Odpoklic add DatumRazklada datetime;

--20.01.2020 - Za potrebe resetiranja in pošiljanja naroèilnice
insert into StatusOdpoklica(Koda, Naziv, Opis, tsIDOseba, ts) values('ERR_ORDER_NO_SEND', 'Naroèilnica NI bila poslana', 'Zaradi napake naroèilnica ni bila poslana', 1, '1.1.2020');
insert into StatusOdpoklica(Koda, Naziv, Opis, tsIDOseba, ts) values('ERR_ADMIN_MAIL', 'Obvestilo admin, NI poslana naroèilnica', 'Obvestilo admin, NI poslana naroèilnica', 1, '1.1.2020');

-- 14.01.2020 - PDO - dodajanje Jezika
create table Jeziki (
	JezikID int not null identity(1,1) primary key,
	Koda varchar(50) not null,
	Naziv varchar(500),
	ts datetime null,	
);

alter table Stranka_OTP add JezikID int null,
constraint FK_Jezik foreign key(JezikID) references Jeziki(JezikID);

Insert into Jeziki values('ANG', 'Anglešèina', '01.01.2020');
Insert into Jeziki values('SLO', 'Slovenšèina', '01.01.2020');
Insert into Jeziki values('HRV', 'Hrvašèina', '01.01.2020');

/*Martin 04.02.2020*/
alter table Jeziki
add KodaJezik varchar(6) null;

update Jeziki set KodaJezik = 'en-US' where Koda = 'ANG';
update Jeziki set KodaJezik = 'sl-SI' where Koda = 'SLO';
update Jeziki set KodaJezik = 'hr-HR' where Koda = 'HRV';

/* Boris 03.03.2020 */
alter table OdpoklicPozicija add StPalet decimal (18,2);
alter table KontaktneOsebe_OTP add NazivPodpis varchar(300)
alter table KontaktneOsebe_OTP add Naziv varchar(300)
