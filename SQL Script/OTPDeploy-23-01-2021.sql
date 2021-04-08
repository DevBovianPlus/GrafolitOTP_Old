alter table OdpoklicKupecPozicija add ZaporednaStevilka int;
alter table OdpoklicKupecPozicija add Vrednost decimal(18,3)
alter table OdpoklicKupecPozicija drop column TipID;
alter table OdpoklicKupecPozicija add TipVnosa int;

alter table OdpoklicKupec add OdpoklicKupecStevilka int;

ALTER TABLE OdpoklicKupec DROP CONSTRAINT FK_Stranka_OdpoklicKupec;
alter table OdpoklicKupec drop column PrevoznikID;
alter table OdpoklicKupec add RazpisPozicijaID int;
ALTER TABLE OdpoklicKupec ADD CONSTRAINT FK_Odpoklic_RazpisPozicija FOREIGN KEY (RazpisPozicijaID) REFERENCES RazpisPozicija(RazpisPozicijaID);
