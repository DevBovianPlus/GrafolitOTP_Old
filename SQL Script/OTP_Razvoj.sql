select * from StatusOdpoklica

select * from Odpoklic order by 1 desc

select * from RazpisPozicija where RelacijaID = 212 and Cena>0 order by cena, ts desc

select * from RazpisPozicija where TenderID =694

select * from Relacija R inner join RazpisPozicija RP on R.RelacijaID = RP.RelacijaID where R.Naziv like ('%AJDOVŠÈINA%')
select DISTINCT RAZ.Naziv from Razpis RAZ  
 inner join RazpisPozicija RP on RAZ.RazpisID = RP.RazpisID
 inner join Relacija R on RP.RelacijaID = R.RelacijaID where R.Naziv like ('%AJDOVŠÈINA%')

select * from Odpoklic where LastenPrevoz=1 and DobaviteljID not in (73, 20)  order by 1 desc

select * from Odpoklic

select * from Osebe_OTP

update Odpoklic set LastenPrevoz = 0, Opis = Opis + ' - Update zaradi lastnega prevoza '  where LastenPrevoz=1 and DobaviteljID not in (73, 20) 

select * from Odpoklic where  DobaviteljUrediTransport=1 order by 1 desc
select * from Odpoklic where  DobaviteljUrediTransport<>1 order by 1 desc

select * from Odpoklic where ts >= '2019-10-10' and ts <= '2020-10-10'
select RelacijaID, COUNT(*) from Odpoklic where ts >= '2019-10-10' and ts <= '2020-10-10'
group by RelacijaID
order by 2 desc

select RelacijaID, COUNT(*) from RazpisPozicija where ts >= '2019-10-10' and ts <= '2020-10-10'
group by RelacijaID
order by 2 desc

select * from Razpis order by 1 desc

select * from Razpis where DatumRazpisa is null

alter table Razpis add PodatkiZaExcell varchar(max);

SELECT MIN(LEN(Naziv)) FROM Relacija;

select * from Relacija where Len(Naziv) = 9

update RazpisPozicija set ZbrirnikTonID = null

update Nastavitve set PosiljanjePoste=1

select * from SystemEmailMessage_PDO order by 1 desc

select * from Nastavitve
select * from ZbirnikTon
select * from Stranka_OTP where idStranka in (73,20)
select * from Stranka_OTP where NazivPrvi like '%ROM%'

select * from StatusOdpoklica 

select * from Odpoklic where RelacijaID = 777 order by 1 desc
select * from Odpoklic order by 1 desc
select * from RazpisPozicija order by 1 desc
select * from Odpoklic where OdpoklicStevilka = 5108
update Odpoklic set CenaPrevoza = 1800, DobaviteljUrediTransport = 0, DobaviteljID=32, ZbirnikTonID=11 where OdpoklicStevilka = 5108
select * from StatusOdpoklica

select * from Odpoklic where ts >= '11-15-2020' and ts <= '11-20-2020' and KolicinaSkupno < 20000 order by RelacijaID
select * from Odpoklic where RelacijaID = 159 and ts > '10-01-2020' and ts < '11-11-2020' and KolicinaSkupno < 20000
select * from Odpoklic where RelacijaID = 159 and ts > '10-01-2020' and ts < '11-11-2020' and KolicinaSkupno > 20000
select * from Odpoklic where RelacijaID = 166 and ts > '10-01-2020' and ts < '11-11-2020' and KolicinaSkupno < 20000 and (KupecUrediTransport = 1 or DobaviteljUrediTransport = 1)
select * from Odpoklic where RelacijaID = 434 and ts > '10-01-2020' and ts < '11-11-2020' and KolicinaSkupno < 20000 and (KupecUrediTransport <> 1 and DobaviteljUrediTransport <> 1 and LastenPrevoz <> 1)

select * from Odpoklic where RelacijaID = 548 and ts >= '11-14-2019' and ts <= '11-14-2020' 
and KolicinaSkupno > 20000 and (KupecUrediTransport <> 1 and DobaviteljUrediTransport <> 1 and LastenPrevoz <> 1)
and (StatusID = 4 or StatusID = 6 or StatusID = 7)

select * from Odpoklic where RelacijaID = 592 and ts >= '11-14-2019' and ts <= '11-14-2020' 
and KolicinaSkupno > 20000 and (KupecUrediTransport = 1 or DobaviteljUrediTransport = 1)
and (StatusID = 4 or StatusID = 6 or StatusID = 7)

select * from Odpoklic where RelacijaID = 592 and ts >= '11-14-2019' and ts <= '11-14-2020' 
and KolicinaSkupno > 20000 and LastenPrevoz = 1
and (StatusID = 4 or StatusID = 6 or StatusID = 7)


select * from Odpoklic where ts > '08-30-2019' and ts < '11-11-2020' and KolicinaSkupno > 20000 and RelacijaID = 548

select r.* from Relacija r right join Odpoklic o on r.RelacijaID = o.RelacijaID where  o.ts > '08-30-2019' and o.ts < '11-11-2020'
select * from Relacija where RelacijaID in (select RelacijaID from Odpoklic where  ts > '11-14-2019' and ts < '11-14-2020') 

select * from Relacija order by 1 desc 
select * from Relacija where RelacijaID = 1060
select * from RazpisPozicija where  RelacijaID=1060 and Cena > 0
select * from RazpisPozicija where  RazpisID=694 and StrankaID = 21 and RelacijaID = 1126
select * from Razpis where RazpisID in (select RazpisID from RazpisPozicija where  RelacijaID=929)
select * from RazpisPozicija where  RazpisID=777
select * from Razpis where RazpisID in (select RazpisID from RazpisPozicija where  RelacijaID=929)
select * from Relacija where Naziv like '%(IT) 36040 SAREGO – (HU) 1033 BUDIMPEŠTA - (HU) 4400 NYIREGYHAZA%'
select * from Odpoklic where OdpoklicStevilka = '4674'
select * from Odpoklic where LastenPrevoz = 1 and DobaviteljID <> 20 and UserID not in (6,10)
select * from Stranka_OTP where idStranka in (22,20,41,31,49,56)
select * from Odpoklic order by 1 desc

select * from OdpoklicPozicija where OdpoklicID = 4460

update Odpoklic set StatusID = 1 where OdpoklicID = 4460

select * from Osebe_OTP
update Osebe_OTP set Priimek = 'Dolanc', Email='mojca.dolanc@grafolit.si' where idOsebe = 2

select * from StatusOdpoklica

select * from SystemEmailMessage_OTP order by 1 desc
select * from Razpis where RazpisID=503
select * from RazpisPozicija order by 1 desc
select * from RazpisPozicija where RazpisID = 666
select * from Razpis order by 1 desc

update SystemEmailMessage_NOZ set Status=1 where Status = 1
select * from SystemEmailMessage_OTP order by 1 desc
select * from SystemEmailMessage_NOZ order by 1 desc
select * from SystemEmailMessage_PDO order by 1 desc
select * from SystemEmailMessage order by 1 desc

select * from Relacija order by 1 desc
select * from Relacija where RelacijaID = 122
update Stranka_OTP set Email = 'boris.dolinsek@gmail.com'
select * from Stranka_OTP
select * from Relacija where Naziv like '%(BA) 74250 MAGLAJ%'


select * from RazpisPozicija where RelacijaID=169

	SELECT StrankaID, RelacijaID, COUNT(*) AS St_Relacija_Dobavitelj
	FROM RazpisPozicija
	WHERE ts < '2019-10-01'	
	GROUP BY StrankaID, RelacijaID

select DISTINCT RP.StrankaID, S.NazivPrvi, RP.* from RazpisPozicija RP 
					INNER JOIN Stranka_OTP S on RP.StrankaID = S.idStranka
where RP.ts < '2019-10-01' and RelacijaID=122 order by 1 	

select O.acStatus as Status, OI.adDeliveryDeadline as ZeljeniRokDobave, SUBSTRING(O.acKey, 1, 2) + '-' + SUBSTRING(O.acKey, 3, 3) + '-' + SUBSTRING(O.acKey, 6, 8) as StevilkaDokumenta, 
O.acReceiver as Stranka, MS.DOBAVITELJ as Dobavitelj, I.Kategorija, OI.acIdent as KodaArtikla, OI.acName as NazivArtikla, OI.anQty as NarocenaKolicina, OI.acUM as EnotaMere,OI._addelivery as PotrjeniRokDobave
from Grafolit55SI.dbo.tHE_OrderItem OI 
inner join Grafolit55SI.dbo.tHE_Order O on OI.acKey = O.acKey 
inner join Grafolit55SI.dbo.MS MS on MS.IDENT = OI.acIdent
left join DW.dbo.DIM_Identi I on I.IDENT = OI.acIdent
where acDocType = '0100' 
and (acStatus = ' ' or acStatus = '1' or acStatus = '2' or acStatus = '0') 
and (OI.adDeliveryDeadline >= DATEADD(DAY, -60, CURRENT_TIMESTAMP) and OI.adDeliveryDeadline <= DATEADD(DAY, 30, CURRENT_TIMESTAMP))  

select * from RazpisPozicija 

update SystemEmailMessage_OTP set EmailTo = 'boris.dolinsek@gmail.com', Status = 0 where SystemEmailMessageID in (2369, 2364)

update SystemEmailMessage_OTP set Status = 1 

update Odpoklic set StatusID = 3 where  OdpoklicID in (3853, 3852, 3851)

select P_TransportOrderPDFName from Odpoklic
alter table Odpoklic add P_GetPDFOrderFile datetime
alter table SystemEmailMessage_OTP add Attachments varchar(4000);
alter table PrijavaPrevoznika add OpombaPrijave varchar(4000);

INSERT INTO StatusOdpoklica values('USTVARJENO_NAROCILO', 'Ustvarjeno naroèilo v Pantheon','Naroèilo je bilo uspešno ustvarjeno v Pantheon-u', 1,CURRENT_TIMESTAMP); 
INSERT INTO StatusOdpoklica values('POPRAVLJENO_NAROCILO', 'Popravljeno izdelano naroèilo v OTP','Popravljeno izdelano naroèilo v OTP', 1,CURRENT_TIMESTAMP); 
INSERT INTO StatusOdpoklica values('KREIRAN_PDF', 'Pantheon je kreiral PDF','Pantheon je kreiral PDF,ki se ga pošlje prevozniku', 1,CURRENT_TIMESTAMP); 

select * from RazpisPozicija order by 1 desc
select * from RazpisPozicijaSpremembe order by 1 desc
select * from Razpis order by 1 desc

select * from Grafolit55SI.dbo.tHE_OrderItem order by adTimeIns desc
select * from Grafolit55SI.dbo.tHE_Order where acDocType = '0100' order by adDate desc
select * from Grafolit55SI.dbo.tHE_Order where acDocType = '0200' and acConsignee like 'RŽENIÈNIK JANEZ S.P. - PAKO - ' order by adDate desc
update Grafolit55SI.dbo.tHE_Order set acStatus = 1 where acDocType = '0200' and acConsignee like 'RŽENIÈNIK JANEZ S.P. - PAKO - '
select * from Grafolit55SI.dbo.tHE_Order where acKey='2102500000073' order by adDate desc -- 3.4.2020
select adDate, acConsignee, anValue from Grafolit55SI.dbo.tHE_Order where acDocType = '0100' order by adDate desc
select * from Grafolit55SI.dbo.tHE_OrderItem where acKey = '2102400000193' order by adTimeIns desc -- adDeliveryDeadline = 3.4.2020

select * from Grafolit55SI.dbo.tHE_Order where acKey = '2102400000193' order by adDate desc

select O.acStatus as Status, OI.adDeliveryDeadline as ZeljeniRokDobave, SUBSTRING(O.acKey, 1, 2) + '-' + SUBSTRING(O.acKey, 3, 3) + '-' + SUBSTRING(O.acKey, 6, 8) as StevilkaDokumenta, 
O.acReceiver as Stranka, MS.DOBAVITELJ as Dobavitelj, OI.acIdent as KodaArtikla, OI.acName as NazivArtikla, OI.anQty as NarocenaKolicina, OI.acUM as EnotaMere,OI._addelivery as PotrjeniRokDobave, MS.POREKLO
from Grafolit55SI.dbo.tHE_OrderItem OI 
inner join Grafolit55SI.dbo.tHE_Order O on OI.acKey = O.acKey 
inner join Grafolit55SI.dbo.MS MS on MS.IDENT = OI.acIdent
where acDocType = '0100' 
and (acStatus = ' ' or acStatus = '1' or acStatus = '2' or acStatus = 'Z') 
and (OI.adDeliveryDeadline >= DATEADD(DAY, -60, CURRENT_TIMESTAMP) and OI.adDeliveryDeadline <= DATEADD(DAY, 30, CURRENT_TIMESTAMP)) 
 order by adDate desc

select * from Grafolit55SI.dbo.MS MS

select OI.adDeliveryDeadline, acStatus, * from Grafolit55SI.dbo.tHE_OrderItem OI inner join Grafolit55SI.dbo.tHE_Order O on OI.acKey = O.acKey 
where acDocType = '0100' 
and (acStatus = ' ' or acStatus = '1' or acStatus = '2' or acStatus = 'Z') 
and OI.adDeliveryDeadline is not null
order by adDate desc

select * from DW.dbo.DIM_Identi

select DATEADD(DAY, -60, CURRENT_TIMESTAMP) from Razpis

select * from Grafolit55SI.dbo.tHE_OrderItem OI inner join Grafolit55SI.dbo.tHE_Order O on OI.acKey = O.acKey where acDocType = '0100' and OI.adDeliveryDeadline < '2001000004321' order by adDate desc




update Grafolit55SI.dbo.tHE_OrderItem set _oc = '4441-2' where acKey = '2002500000048'
update Grafolit55SI.dbo.tHE_OrderItem set _oc = '1111-3' where acKey = '2002500000049'

select * from SeznamPozicijOdprtihNarocilnicGledeNaDobavitelja('RŽENIÈNIK JANEZ S.P. - PAKO -') 
select * from SeznamPozicijOdprtihNarocilnicGledeNaDobaviteljaMy('RŽENIÈNIK JANEZ S.P. - PAKO - ') 
																RŽENIÈNIK JANEZ S.P. - PAKO -
																RŽENIÈNIK JANEZ S.P. - PAKO - 
select * from SeznamPozicijNarocilnic10ZaOdpoklic()
select * from Grafolit55SI.dbo.tHE_Order where 

select * from DW.dbo.DIM_Identi_OPT
select * from DW.dbo.DIM_Identi where IDENT = '0731320100044F  '

select * from Razpis order by 1 desc

select * from Osebe_OTP

select * from Grafolit55SI.dbo.tHE_OrderItem order by 1 desc
select * from Grafolit55SI.dbo.vHE_OrderItemDistKeyItem order by 1 desc
select * from DW.dbo.FACT_Optimalna_Zaloga

------------- Seznam nepovezanih faktur ----------------------------

SELECT      LEFT(G.acKey, 2) + '-' + SUBSTRING(G.acKey, 3, 3) + '-' + RIGHT(G.acKey, 6) AS Kljuc,
			G.acKey,
            G.adDate AS Datum,
            G.acCurrency AS Valuta,
            G.acReceiver AS Kupec,
            acPrsn3 AS Prevzemnik,
sum(MI.anQty) as Kolicina,
G.anValue as Vrednost
FROM Grafolit55SI.dbo.tHE_Move G
 left join Grafolit55SI.dbo.tHE_MoveItem MI
on MI.acKey = G.acKey
      LEFT JOIN Grafolit55SI.dbo._epos_GTLink GT
            ON G.ackey = GT.LinkKljuc
WHERE G.acDocType IN ('3600', '3900', '3910', '3920', '3930', '3960')
      AND GT.LinkKljuc IS NULL
and Year(G.adDate) = 2020 and Month(G.adDate) = 12 and Day(G.adDate) > 1
group by LEFT(G.acKey, 2) + '-' + SUBSTRING(G.acKey, 3, 3) + '-' + RIGHT(G.acKey, 6), G.acKey, G.adDate, G.acCurrency, G.acReceiver, acPrsn3, G.anValue



------------- Seznam povezanih faktur ----------------------------

SELECT      LEFT(G.acKey, 2) + '-' + SUBSTRING(G.acKey, 3, 3) + '-' + RIGHT(G.acKey, 6) AS Kljuc,
			G.acKey,
            G.adDate AS Datum,
            G.acCurrency AS Valuta,
            G.acReceiver AS Kupec,
            acPrsn3 AS Prevzemnik,
			GT.ProcTrans, 
			GT.Vrednost,
			GT.VredTrans,
			GT.ProcTransFakt,
			sum(MI.anQty) as Kolicina
FROM Grafolit55SI.dbo.tHE_Move G
 left join Grafolit55SI.dbo.tHE_MoveItem MI
on MI.acKey = G.acKey
      LEFT JOIN Grafolit55SI.dbo._epos_GTLink GT
            ON G.ackey = GT.LinkKljuc
WHERE G.acDocType IN ('3600', '3900', '3910', '3920', '3930', '3960')
      AND GT.NarocKljuc = '2102400000193'
and Year(G.adDate) = 2020 and Month(G.adDate) = 12 and Day(G.adDate) > 1
group by LEFT(G.acKey, 2) + '-' + SUBSTRING(G.acKey, 3, 3) + '-' + RIGHT(G.acKey, 6), G.acKey, G.adDate, G.acCurrency, G.acReceiver, acPrsn3, GT.ProcTrans, GT.Vrednost, GT.VredTrans, GT.ProcTransFakt

select * FROM Grafolit55SI.dbo.tHE_Move where acKey = '2102400000193'
select * FROM Grafolit55SI.dbo.tHE_Move where acKey = '2102400000194'
select * FROM Grafolit55SI.dbo._epos_GTLink where NarocKljuc = '2102400000065'

select * FROM Grafolit55SI.dbo._epos_GTLink order by 1 desc

select * from SeznamNepovezanihFaktur()
select * from OdpoklicKupecPozicija order by 1 desc
select * from OdpoklicKupec order by 1 desc
select * from ZbirnikTon order by 1 desc
exec SeznamPovezanihFakturByOrderNo '2102400000131'

update Osebe_NOZ set NOZDostop = 1,  where UporabniskoIme='AljosaS'

select * from RazpisPozicijaSpremembe order by 1 desc

select * from Odpoklic where RelacijaID = 730 order by 1 desc
select * from OdpoklicPozicija where  OdpoklicID = 5835 order by 1 desc
select * from RazpisPozicija where RelacijaID = 730 and Cena > 0 order by 1 desc
select * from RazpisPozicija where RelacijaID = 502 and Cena > 0 order by 1 desc
select * from RazpisPozicija where RelacijaID = 1174 and Cena > 0 order by 1 desc
select * from Relacija where RelacijaID = 1174
select * from Relacija where Naziv like '%NAKLO%'

select * from RazpisPozicija where ZbirnikTonID <> 10 and Cena>0 order by 1 desc

select * from Grafolit55SI.dbo.tHE_OrderItem order by adTimeIns desc
select * from Grafolit55SI.dbo.tHE_Order order by adTimeIns desc
select * from Grafolit55SI.dbo.tHE_Move order by adTimeIns desc

select * from OdpoklicKupec order by 1 desc