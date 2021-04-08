delete from RazpisPozicija where RazpisID in (select RazpisID from RazpisPozicija where RazpisID in (691, 692, 693))
delete from Razpis where RazpisID in (691, 692, 693)

