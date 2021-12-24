select * from StatusOdpoklica

select * from OdpoklicKupec order by 1 desc

update OdpoklicKupec set StatusID=4 where Stevilkanarocilnica='2102400001103'

select * from RazpisPozicija where RelacijaID = 1137 and Cena>0 order by cena, ts desc
select OdpoklicStevilka, * from Odpoklic where RelacijaID = 1182
select OdpoklicKupecStevilka,* from OdpoklicKupec where RelacijaID = 1137

select * from RazpisPozicija where TenderID =694

select * from Relacija R inner join RazpisPozicija RP on R.RelacijaID = RP.RelacijaID where R.Naziv like ('%(IT) 10060 NONE - (HR) 10 000 ZAGREB%')
select * from Relacija R where R.Naziv like ('%(AT) 8101 GRATKORN - (SLO) 3310 ŽALEC%')
select * from Relacija R where R.Naziv like ('%MALACKY%')
select * from Relacija R inner join RazpisPozicija RP on R.RelacijaID = RP.RelacijaID where R.Naziv like ('%(SLO) 1433 RADEČE - (SK) 901 00 MALACKY%')

select * from Razpis where RazpisID in (select RazpisID from Relacija R inner join RazpisPozicija RP on R.RelacijaID = RP.RelacijaID where R.Naziv like ('%(SLO) 1433 RADEČE - (SK) 901 00 MALACKY%'))

select DISTINCT S.NazivPrvi, RAZ.Naziv, RAZ.DatumRazpisa from Razpis RAZ  
 inner join RazpisPozicija RP on RAZ.RazpisID = RP.RazpisID
 inner join Relacija R on RP.RelacijaID = R.RelacijaID 
 inner join Stranka_OTP S on S.idStranka = RP.StrankaID
 where R.Naziv like ('%(IT) 10060 NONE - (SLO) 1310 RIBNICA%') and S.NazivPrvi = 'HART d.o.o.'
 order by RAZ.DatumRazpisa desc

 select DISTINCT O.OdpoklicStevilka, O.ts as DatumOdpoklica, S.NazivPrvi as NazivPrevoznik,R.Naziv as NazivRelacija,RAZ.Naziv as NazivRazpisa, RAZ.DatumRazpisa, RP.Cena from Odpoklic O 	
	inner join Stranka_OTP S on O.DobaviteljID = S.idStranka
	inner join Relacija R on O.RelacijaID = R.RelacijaID 
	inner join RazpisPozicija RP on RP.RazpisPozicijaID = O.RazpisPozicijaID
	inner join Razpis RAZ on RAZ.RazpisID = RP.RazpisID
where S.NazivPrvi like '%INTEREUROPA d.d.%' and R.Naziv like '%(IT) 10060 NONE - (SLO) 1310 RIBNICA%' and O.OdpoklicStevilka = '6231'

 select DISTINCT O.OdpoklicStevilka, O.ts as DatumOdpoklica, O.CenaPrevoza, 
 O.ZbirnikTonID,O.RazpisPozicijaID, S.idStranka, S.NazivPrvi as NazivPrevoznik,O.RelacijaID,R.Naziv as NazivRelacija,RAZ.Naziv as NazivRazpisa, 
 RAZ.DatumRazpisa, RP.Cena as RazpisnaCena, ZT.Naziv  from Odpoklic O 	
	inner join Stranka_OTP S on O.DobaviteljID = S.idStranka
	inner join Relacija R on O.RelacijaID = R.RelacijaID 
	inner join RazpisPozicija RP on RP.RazpisPozicijaID = O.RazpisPozicijaID 
	inner join Razpis RAZ on RAZ.RazpisID = RP.RazpisID
	inner join ZbirnikTon ZT on ZT.ZbirnikTonID = O.ZbirnikTonID
where O.OdpoklicStevilka = '7138'

-- preverjamo analizo za Odpoklic
 select DISTINCT O.OdpoklicStevilka, O.ts as DatumOdpoklica, O.ConfirmTS, O.CenaPrevoza, 
 O.ZbirnikTonID, ZT.Naziv as ZbirnikTonOdpoklic ,O.RazpisPozicijaID, S.idStranka, S.NazivPrvi as NazivPrevoznik,O.RelacijaID,R.Naziv as NazivRelacija
 ,RAZ.Naziv as NazivRazpisa, RAZ.DatumRazpisa, 
 RP.Cena as RazpisnaCena from Odpoklic O 	
	inner join Stranka_OTP S on O.DobaviteljID = S.idStranka
	inner join Relacija R on O.RelacijaID = R.RelacijaID 
	left outer join RazpisPozicija RP on RP.RazpisPozicijaID = O.RazpisPozicijaID and RP.ZbirnikTonID = O.ZbirnikTonID
	inner join RazpisPozicija RP2 on RP2.RazpisPozicijaID = O.RazpisPozicijaID
	inner join ZbirnikTon ZT on ZT.ZbirnikTonID = O.ZbirnikTonID
	left outer join Razpis RAZ on RAZ.RazpisID = RP2.RazpisID
where O.OdpoklicStevilka = '7755'

-- preverjamo analizo za OdpoklicKupcem
 select DISTINCT O.OdpoklicKupecStevilka, OdpoklicKupecID, O.ts  as DatumOdpoklica, O.CenaPrevoza, O.ZbirnikTonID,O.RazpisPozicijaID, 
 S.NazivPrvi as NazivPrevoznik,
 O.RelacijaID,R.Naziv as NazivRelacija
 ,RAZ.Naziv as NazivRazpisa, RAZ.DatumRazpisa, RAZ.RazpisID, 
 RP.Cena
 ,ZT.Naziv, RP2.ZbirnikTonID 
 from OdpoklicKupec O 		
	inner join Relacija R on O.RelacijaID = R.RelacijaID 
	left outer join RazpisPozicija RP on RP.RazpisPozicijaID = O.RazpisPozicijaID and RP.ZbirnikTonID = O.ZbirnikTonID
	left outer join Stranka_OTP S on RP.StrankaID = S.idStranka	
	inner join RazpisPozicija RP2 on RP2.RazpisPozicijaID = O.RazpisPozicijaID
	inner join ZbirnikTon ZT on ZT.ZbirnikTonID = O.ZbirnikTonID
	left outer join Razpis RAZ on RAZ.RazpisID = RP2.RazpisID
where O.OdpoklicKupecStevilka = '1868'

select * from Razpis where RazpisID = 1335

select * from Odpoklic order by 1 desc
select * from OdpoklicKupec where OdpoklicKupecStevilka = '1763'
select * from OdpoklicKupecPozicija where OdpoklicKupecID = 1806 
select * from Odpoklic where OdpoklicStevilka = '8542'
select * from Odpoklic where OdpoklicStevilka = '7592'
58570
update Odpoklic set RazpisPozicijaID = null  where OdpoklicStevilka = '8542'
select * from Stranka_OTP where idStranka = 50
select * from RazpisPozicija where RazpisPozicijaID = 2878  
select * from RazpisPozicija where Cena>0 and  RelacijaID = 1196 and ZbirnikTonID = 6 order by ts desc
select * from RazpisPozicija where Cena>0 and  RelacijaID = 1421 and ZbirnikTonID = 6 order by Cena 
select * from RazpisPozicija where RelacijaID = 1214 and Cena > 0 and ZbirnikTonID = 3
select * from ZbirnikTon where ZbirnikTonID = 5
select * from Razpis where RazpisID = 1110
select * from RazpisPozicija where RelacijaID = 64 and Cena>0 and StrankaID = 50 order by cena, ts desc
select * from Relacija where RelacijaID = 1236
select * from RazpisPozicija where RelacijaID = 166 and Cena > 0 order by Cena 
select * from Razpis where RazpisID = 25
select * from AktivnostUporabnika

select * from Razpis where Naziv = '%32035 BELLUNO%'
select * from Relacija where Naziv like '%(SLO) 3310 ŽALEC - (SL.KONJICE + SL.BISTRICA) (SLO) 2000 MARIBOR%'
select * from Odpoklic where tmpTenderDate is not null

UPDATE Odpoklic SET tmpTenderDate='44364' where OdpoklicStevilka = 'D6800';
UPDATE Odpoklic SET tmpTenderDate='2021' where OdpoklicStevilka = 'D6800';
UPDATE Odpoklic SET tmpTenderDate='2021/06/17' where OdpoklicStevilka = '6800';

select * from OdpoklicKupec order by ts desc

select * from Razpis where GeneriranTender is null

 select * from Odpoklic where RelacijaID = 1182
 select * from OdpoklicPozicija OP order by 1 desc
 
select * from RazpisPozicija where RelacijaID = 1214 and cena > 0

 select * from OdpoklicKupec where OdpoklicKupecID = 2052
 select * from OdpoklicKupec where RelacijaID = 1120

 select DISTINCT RAZ.Naziv from Razpis RAZ  
 inner join RazpisPozicija RP on RAZ.RazpisID = RP.RazpisID
 inner join Relacija R on RP.RelacijaID = R.RelacijaID 
 inner join Stranka_OTP S on RP.DobaviteljID = S.idStranka
 where  R.RelacijaID = 1182

select * from Odpoklic where LastenPrevoz=1 and DobaviteljID not in (73, 20)  order by 1 desc

select * from Odpoklic
select * from OdpoklicPozicija
select * from RazpisPozicija
select * from RazpisPozicija

select * from Osebe_OTP
select * from Stranka_OTP

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
select * from Relacija where RelacijaID = 949
select * from RazpisPozicija where  RelacijaID=1120 and Cena > 0 and ZbirnikTonID = 10
select * from RazpisPozicija where  RelacijaID=181 and ZbirnikTonID=5
select * from RazpisPozicija where  RelacijaID=170 and ZbirnikTonID<>10 and Cena > 0
select * from RazpisPozicija where  RazpisID=694 and StrankaID = 21 and RelacijaID = 1126
select * from Razpis where RazpisID in (select RazpisID from RazpisPozicija where  RelacijaID=929)
select * from RazpisPozicija where  RazpisID=777
select * from RazpisPozicija where  RazpisID=83068
select * from Razpis where RazpisID in (select RazpisID from RazpisPozicija where  RelacijaID=929)
select * from Relacija where Naziv like '%(SLO) 1433 RADEČE - (MAK) 1000 SKOPJE%'
select * from Odpoklic where OdpoklicStevilka = '4674'
select * from Odpoklic where RelacijaID=1185 and ts >= '06-06-2020' and ts <= '06-06-2021' and StatusID in (4, 6, 7) and KolicinaSkupno < 20000
select * from Odpoklic where RelacijaID=1182 and ts >= '06-06-2020' and ts <= '06-06-2021' and StatusID in (4, 6, 7) and KolicinaSkupno < 20000
select * from Odpoklic where LastenPrevoz = 1 and DobaviteljID <> 20 and UserID not in (6,10)
select * from Stranka_OTP where idStranka in (22,20,41,31,49,56,181,75,52)
select * from Odpoklic order by 1 desc

select * from RazpisPozicija where RelacijaID = 91 and Cena > 0 and ZbirnikTonID = 10 order by Cena asc

select * from RazpisPozicija rp inner join Stranka_OTP s on rp.StrankaID = s.idStranka where rp.RelacijaID = 346 and rp.ZbirnikTonID = 3 and s.Activity = 1
select * from RazpisPozicija rp where rp.RelacijaID = 346 and rp.ZbirnikTonID = 5
select * from ZbirnikTon

select * from Relacija where Naziv like '%RADEČE %'

select * from OdpoklicKupec where OdpoklicKupecStevilka = 164
select * from Relacija where RelacijaID = 76
select * from Razpis where RazpisID = 694
select * from RazpisPozicija where  RelacijaID = 76 and StrankaID = 7
select * from RazpisPozicija where  StrankaID = 75 and Cena > 0
select * from RazpisPozicija where  RazpisPozicijaID = 83068
select * from Relacija where RelacijaID = 1120

select * from GetPantheonUsersNOZ()

select RP.Cena, R.Naziv, S.NazivPrvi, * from RazpisPozicija RP inner join Stranka_OTP S on RP.StrankaID = S.idStranka inner join Relacija R on RP.RelacijaID = R.RelacijaID where  Cena = 1
select * from Razpis where RazpisID = 613
select * from Stranka_OTP where NazivPrvi like '%LKW%'

select * from RazpisPozicija where RelacijaID = 1120
select * from RazpisPozicija where RazpisPozicijaID = 83068


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
select * from DeliveryNoteItem order by 1 desc

select * from Grafolit55SI.dbo.tHE_OrderItem order by adTimeIns desc
select * from Grafolit55SI.dbo.tHE_Order where acDocType = '0100' order by adDate desc
select * from Grafolit55SI.dbo.tHE_Order where acDocType = '0200' and acConsignee like 'RŽENIÈNIK JANEZ S.P. - PAKO - ' order by adDate desc
update Grafolit55SI.dbo.tHE_Order set acStatus = 1 where acDocType = '0200' and acConsignee like 'RŽENIÈNIK JANEZ S.P. - PAKO - '
select * from Grafolit55SI.dbo.tHE_Order where acKey='2102400000142' order by adDate desc -- 3.4.2020
select adDate, acConsignee, anValue from Grafolit55SI.dbo.tHE_Order where acDocType = '0100' order by adDate desc
select * from Grafolit55SI.dbo.tHE_OrderItem where acKey = '2102400000142' order by adTimeIns desc -- adDeliveryDeadline = 3.4.2020

select * from Grafolit55SI.dbo.tHE_Order where acKey = '2102400000132' order by adDate desc

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

select * from SeznamPozicijOdprtihNarocilnicGledeNaDobavitelja('BURGO GROUP s.p.a.            ') 
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

USE Grafolit55SI DBCC DBREINDEX tHE_MoveItem

SELECT      LEFT(G.acKey, 2) + '-' + SUBSTRING(G.acKey, 3, 3) + '-' + RIGHT(G.acKey, 6) AS Kljuc,
G.acKey,
            G.adDate AS Datum,
            G.acCurrency AS Valuta,
            G.acReceiver AS Kupec,
            acPrsn3 AS Prevzemnik,
sum(MI.anQty) as Kolicina, 
G.anValue as ZnesekFakture
FROM Grafolit55SI.dbo.tHE_Move G
 left join Grafolit55SI.dbo.tHE_MoveItem MI
on MI.acKey = G.acKey
      LEFT JOIN Grafolit55SI.dbo._epos_GTLink GT
            ON G.ackey = GT.LinkKljuc
WHERE G.acDocType IN ('3600', '3900', '3910', '3920', '3930', '3960','3950','3940')
      AND GT.LinkKljuc IS NULL 
and G.adDate > DATEADD(Year,-1,GETDATE())
group by LEFT(G.acKey, 2) + '-' + SUBSTRING(G.acKey, 3, 3) + '-' + RIGHT(G.acKey, 6), G.acKey, G.adDate, G.acCurrency, G.acReceiver, acPrsn3, G.anValue

select count(*) FROM Grafolit55SI.dbo.tHE_Move
select * FROM Grafolit55SI.dbo.tHE_Move where acKey = '2102400000132'
select * FROM Grafolit55SI.dbo.tHE_Move where acKey = '2102400000132'
select * FROM Grafolit55SI.dbo._epos_GTLink where NarocKljuc = '2102400000135'
select * from Grafolit55SI.dbo.tHE_Order where acKey = '2102400000136' order by adDate desc
select * from Relacija where Naziv = '(HR) 10 000 ZAGREB - (SLO) 5270 AJDOVŠČINA'


select * FROM Grafolit55SI.dbo._epos_GTLink order by 1 desc

select * from SeznamNepovezanihFaktur()
select * from OdpoklicKupecPozicija order by 1 desc
select * from OdpoklicKupec order by 1 desc
select * from ZbirnikTon order by 1 desc
exec SeznamPovezanihFakturByOrderNo '2102400000132'

update Osebe_NOZ set NOZDostop = 1,  where UporabniskoIme='AljosaS'

select * from RazpisPozicijaSpremembe order by 1 desc

select * from Odpoklic where RelacijaID = 730 order by 1 desc
select * from OdpoklicPozicija where  OdpoklicID = 5835 order by 1 desc
select * from RazpisPozicija where RelacijaID = 730 and Cena > 0 order by 1 desc
select * from RazpisPozicija where RelacijaID = 502 and Cena > 0 order by 1 desc
select * from RazpisPozicija where RelacijaID = 129 and Cena > 0 order by 1 desc
select * from RazpisPozicija where RelacijaID = 129 and Cena > 0 order by 1 desc
select * from Relacija where RelacijaID = 1283
select * from Relacija where Naziv like '%RIBNICA%'

select * from RazpisPozicija where ZbirnikTonID = 13 and RelacijaID = 346 order by 1 desc
select * from RazpisPozicija where Cena > 0 order by RelacijaID desc

select * from Grafolit55SI.dbo.tHE_OrderItem order by adTimeIns desc
select * from Grafolit55SI.dbo.tHE_Order order by adTimeIns desc
select * from Grafolit55SI.dbo.tHE_Move order by adTimeIns desc

select * from OdpoklicKupec order by 1 desc
select * from Odpoklic order by 1 desc



 select DISTINCT O.OdpoklicKupecStevilka, O.ts as DatumOdpoklica, O.CenaPrevoza, O.ZbirnikTonID, S.NazivPrvi as NazivPrevoznik,
 O.RelacijaID,R.Naziv as NazivRelacija, RAZ.RazpisID, RAZ.Naziv as NazivRazpisa, RAZ.DatumRazpisa, RP.Cena, ZT.Naziv 
 from OdpoklicKupec O 		
	inner join Relacija R on O.RelacijaID = R.RelacijaID 
	left outer join RazpisPozicija RP on RP.RazpisPozicijaID = O.RazpisPozicijaID and RP.ZbirnikTonID = O.ZbirnikTonID
	inner join RazpisPozicija RP2 on RP2.RazpisPozicijaID = O.RazpisPozicijaID
	inner join Stranka_OTP S on RP.StrankaID = S.idStranka
	left outer join Razpis RAZ on RAZ.RazpisID = RP2.RazpisID
	inner join ZbirnikTon ZT on ZT.ZbirnikTonID = O.ZbirnikTonID
where O.OdpoklicKupecStevilka = '484'