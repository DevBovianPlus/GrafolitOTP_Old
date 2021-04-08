DECLARE @p_XMLOrder XML = N'<?xml version="1.0" encoding="utf-16" standalone="yes"?>
<TransportOrder>
  <OrderNo></OrderNo>
  <timestamp>27.3.2020 9:22:27</timestamp>
  <DocType>0240</DocType>  
  <Supplier>INTEREUROPA d.d.</Supplier>
  <Buyer>Grafo Lit d.o.o.</Buyer>
  <OrderDate>27.3.2020 9:22:27</OrderDate>  
  <Route/>
  <PrintType>A0U</PrintType>
  <OrderNote>test test</OrderNote>
  <Products>
    <Product>
      <Department>15 - SKLADIŠČE MALOPRODAJA</Department>
      <Ident>STORITEV</Ident>
      <Name>VRBJE-LJUBLJANA</Name>
      <Qty>2</Qty>
      <Price>176,22</Price>
      <Rabat>10,00</Rabat>      
    </Product>
	<Product>
      <Department>15 - SKLADIŠČE MALOPRODAJA</Department>
      <Ident>STORITEV</Ident>
      <Name>VRBJE-LJUBLJANA</Name>
      <Qty>3</Qty>
      <Price>76,22</Price>
      <Rabat>5,25</Rabat>      
    </Product>
	<Product>
      <Department>15 - SKLADIŠČE MALOPRODAJA</Department>
      <Ident>STORITEV</Ident>
      <Name>VRBJE-LJUBLJANA</Name>
      <Qty>1</Qty>
      <Price>17,22</Price>
      <Rabat>45,00</Rabat>      
    </Product>
  </Products>
</TransportOrder>'


DECLARE @p_XMLInvoice XML = N'<?xml version="1.0" encoding="utf-16" standalone="yes"?>
<ConnectedInvoices>
  <OrderNo></OrderNo> številka ki jo zgenerira prejšnji XML2002100002696
  <Invoices>
    <Invoice>
     <Action>ADD</Action>
     <Key>2039000010271</Key>
     <TransportDate>29.12.2020</TransportDate>
     <Buyer>PARTNER GRAF D.O.O.</Buyer>
     <Reciever>PARTNER GRAF D.O.O.</Reciever>
     <Currency>EUR</Currency>
     <InvoiceValue>1336,92</InvoiceValue>
     <TransportValue>1336,92</TransportValue>
     <TransportPercent>3,62</TransportPercent>
   </Invoice>
<Invoice>
   <Action>UPDATE</Action>
   <Key>2039000010270</Key>
   <TransportDate>29.12.2020</TransportDate>
   <Buyer>SINEGRAF D.O.O.</Buyer>
   <Reciever>MOZAIK TISK D.O.O.</Reciever>
   <Currency>EUR</Currency>
   <InvoiceValue>2123,56</InvoiceValue>
   <TransportValue>71,28</TransportValue>
   <TransportPercent>3,36</TransportPercent>
 </Invoice>
 <Invoice>
    <Action>DELETE</Action>
    <Key>2039000010267</Key>
    <TransportDate>29.12.2020</TransportDate>
    <Buyer>TOME LB D.O.O.</Buyer>
    <Reciever>TOME LB D.O.O.</Reciever>
    <Currency>EUR</Currency>
    <InvoiceValue>403,96</InvoiceValue>
    <TransportValue>14,61</TransportValue>
    <TransportPercent>3,62</TransportPercent>
  </Invoice>
  <Invoice>
     <Action>DELETE</Action>avto
     <Key>2039000010265</Key>
     <TransportDate>29.12.2020</TransportDate>
     <Buyer>MLEKARNA CELEIA D.O.O.</Buyer>
     <Reciever>MLEKARNA CELEIA D.O.O.</Reciever>
     <Currency>EUR</Currency>
     <InvoiceValue>1160,4</InvoiceValue>
     <TransportValue>41,97</TransportValue>
     <TransportPercent>3,62</TransportPercent>
   </Invoice> 
 </Invoices>
</ConnectedInvoices>
'

DECLARE
	@p_cKey CHAR(13),
	@p_cError VARCHAR(8000),
	@p_XMLOrder XML = N'',
	@p_XMLInvoice XML = N''

EXEC _upJM_CreateSupplierOrderAndLinkInvoice @p_XMLOrder, @p_XMLInvoice, @p_cKey OUTPUT, @p_cError OUTPUT

SELECT @p_cKey, @p_cError


--select * from _utJM_CreateSupplierOrderAndLinkInvoice_Log
--truncate table _utJM_CreateSupplierOrderAndLinkInvoice_Log

--select top 10 * from the_order where acdoctype = '0240' order by ackey desc
/*
select * from _epos_GTLink where NarocKljuc = '2102100000052'
select * from the_move where ackey in ('2039000010271','2039000010267','2039000010265')*/




