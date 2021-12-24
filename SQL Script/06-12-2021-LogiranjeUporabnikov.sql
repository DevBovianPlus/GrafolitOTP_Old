select * from AktivnostUporabnika order by 1 desc
select * from AktivnostUporabnikaStatus order by 1 desc
INSERT INTO AktivnostUporabnikaStatus(Koda, Naziv, Opis, TS) values('LOGGED','Logged in','Logged in',GETDATE())
exec SeznamPovezanihFakturByOrderNo '2102400000132'

