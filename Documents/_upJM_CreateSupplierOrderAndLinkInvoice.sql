USE [TESTJM_Grafolit55SI_10012021]
GO

/****** Object:  StoredProcedure [dbo].[_upJM_CreateSupplierOrderAndLinkInvoice]    Script Date: 12.1.2021 9:57:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[_upJM_CreateSupplierOrderAndLinkInvoice]
(
	@p_XMLOrder XML, --XML naroèilo z vsemi parametri.
	@p_XMLInvoice XML, --XML faktur z vsemi parametri.
	@p_cKey CHAR(13) OUTPUT, --Izhodni parameter s kljuèem dokumenta. 
	@p_cError VARCHAR(8000) OUTPUT --Izhodni parameter s podatkom o napaki.
)
AS
BEGIN
	/*Spremenljivke.*/
	DECLARE 		
		@cDocType CHAR(4),
		@cConsignee VARCHAR(30),
		@cConsignee_CHAR CHAR(30),
		@cReceiver VARCHAR(30),
		@cReceiver_CHAR CHAR(30),
		@dDate SMALLDATETIME,
		@dDeliveryDeadline SMALLDATETIME,
		@cReport CHAR(3),
		--@dDateDue SMALLDATETIME,
		@cExportFolder VARCHAR(1000),
		@cDept CHAR(30),
		@cIdent CHAR(16),
		@cName VARCHAR(80),
		@nQty DECIMAL(19,6),
		@nPrice FLOAT,
		@nRebate FLOAT, 
		@dDeliveryDeadlinePos SMALLDATETIME,
		@cDeptPos CHAR(30),
		@cNotePos VARCHAR(2000),
		@cNote VARCHAR(MAX),
		@nNo INT,
		@cWarehouse CHAR(30) = '',
		@cUserId VARCHAR(255),
		@cOrderNo varchar(20),
		@cOrderNoFac varchar(20),
		@cOrderNoFacKey varchar(13),
		@cFacKey varchar(13),
		@cAction varchar(13),
		@nClerk INT,
		@cTranStart VARCHAR(1) = 'F'
	
	/*LOG*/
	if object_id ('_utJM_CreateSupplierOrderAndLinkInvoice_Log') is null
	CREATE TABLE _utJM_CreateSupplierOrderAndLinkInvoice_Log
	(
		p_XMLOrder XML,
		p_XMLInvoice XML,
		adTimeIns DATETIME
	)

	/*Privzete vrednosti.*/
    SET @p_cError = ''

	/*Zapis v Log tabelo.*/
	INSERT INTO _utJM_CreateSupplierOrderAndLinkInvoice_Log ( p_XMLOrder, p_XMLInvoice, adTimeIns )
	SELECT @p_XMLOrder, @p_XMLInvoice, GETDATE()

	IF @@TRANCOUNT = 0
    BEGIN
        BEGIN TRANSACTION _upJM_CreateSuppOrderAndLink
        SET @cTranStart = 'T'
    END
	BEGIN TRY
		
		if @p_XMLOrder.exist('/TransportOrder') > 0
		BEGIN

			/*Pridobivanje parametrov naroèila.*/
			SELECT TOP 1 
				@cOrderNo = X.value('OrderNo[1]', 'VARCHAR(20)'), 
				@cDocType = X.value('DocType[1]', 'CHAR(4)'),
				@cConsignee = X.value('Supplier[1]','VARCHAR(30)'),
				@cReceiver = X.value('Buyer[1]','VARCHAR(30)'),
				@dDate = CONVERT(SMALLDATETIME, REPLACE(X.value('OrderDate[1]','VARCHAR(255)'), '. ', '.'), 104),
				--@dDeliveryDeadline = CONVERT(SMALLDATETIME, REPLACE(X.value('DeliveryDate[1]','VARCHAR(255)'), '. ', '.'), 104),
				@cReport = X.value('PrintType[1]', 'CHAR(3)'), 
				@cNote = X.value('OrderNote[1]','VARCHAR(8000)')
			FROM @p_XMLOrder.nodes('/TransportOrder') AS X(X) 

			-- Èe ne obstaja OrderNo se kreira novo 024 naroèilo
			IF isnull(@cOrderNo,'') = ''
			BEGIN

				IF LTRIM(RTRIM(ISNULL(@cReceiver, ''))) = ''
					SET @cReceiver = @cConsignee
			
				IF @cDept IS NULL 
					SET @cDept = ''

				SET @dDate = CAST(CAST(@dDate AS DATE) AS SMALLDATETIME)
				SET @dDeliveryDeadline = CAST(CAST(@dDeliveryDeadline AS DATE) AS SMALLDATETIME)

				/*Kontrola parametrov.*/
				SET @p_cError = ''

				IF (SELECT COUNT(*) FROM tPA_Reports WHERE acReport = @cReport) = 0 OR LTRIM(RTRIM(ISNULL(@cReport, ''))) = ''
					SET @p_cError = 'Šifra izpisa ' + LTRIM(RTRIM(ISNULL(@cReport, ''))) + ' ne obstaja!'
				IF (SELECT COUNT(*) FROM tPA_SetDocType WHERE acDocType = @cDocType) = 0
					SET @p_cError = 'Vrsta dokumenta ' + LTRIM(RTRIM(ISNULL(@cDocType, ''))) + ' ne obstaja!'
				IF @cDocType NOT IN ('0240')
					SET @p_cError = 'Vrsta dokumenta mora biti 024!'
				IF (SELECT COUNT(*) FROM tHE_SetSubj WHERE CAST(acSubject AS VARCHAR(30)) = @cConsignee) = 0
					SET @p_cError = 'Dobavitelj ' + LTRIM(RTRIM(ISNULL(@cConsignee, ''))) + ' ne obstaja v šifrantu subjektov!'
				IF (SELECT COUNT(*) FROM tHE_SetSubj WHERE acSubject = @cReceiver) = 0
					SET @p_cError = 'Kupec ' + LTRIM(RTRIM(ISNULL(@cReceiver, ''))) + ' ne obstaja v šifrantu subjektov!'
				IF ISNULL(@dDate, '19000101') = '19000101'
					SET @p_cError = 'Datum naroèila mora biti doloèen!'
				IF (SELECT COUNT(*) FROM tHE_SetSubj WHERE CAST(acSubject AS VARCHAR(30)) = @cDept AND acDept = 'T') = 0 AND LTRIM(RTRIM(@cDept)) <> ''
					SET @p_cError = 'Oddelek ' + LTRIM(RTRIM(ISNULL(@cDept, ''))) + ' ne obstaja v šifrantu!'

				IF ISNULL(@p_cError, '') <> ''
					RAISERROR(@p_cError, 16, 1)

				--SET @cConsignee_CHAR = CAST(@cConsignee AS CHAR(30))
				--SET @cReceiver_CHAR = CAST(@cReceiver AS CHAR(30))
				SET @nClerk = ISNULL(NULLIF(dbo.fPA_UserGetanUserId(@cUserId), 0), 1)

				/*Kreiranje glave naroèila.*/
				--EXEC pHE_OrderCreAll @cDocType, @cConsignee, @cReceiver_CHAR, @cWarehouse, @dDate, @nClerk, '', @p_cKey OUTPUT
				EXEC pHE_OrderCreAll @cDocType, @cConsignee, @cReceiver, @cWarehouse, @dDate, @nClerk, '', @p_cKey OUTPUT

				IF NOT EXISTS(SELECT acKey FROM tHE_Order WHERE acKey = @p_cKey)
				BEGIN
					SET @p_cError = 'Napaka pri kreiranju glave naroèila!'
					RAISERROR(@p_cError, 16, 1)
				END
		
				UPDATE tHE_Order
				SET acCurrency = 'EUR', 
					--anDaysForPayment = DATEDIFF(DAY, @dDate, ISNULL(@dDateDue, @dDate)),
					acDept = @cDept,
					acNote = @cNote
				WHERE acKey = @p_cKey

				/*Kreiranje pozicij.*/
				DECLARE cPositions CURSOR LOCAL FAST_FORWARD FOR
					SELECT 
						X.value('Ident[1]', 'CHAR(16)'),
						X.value('Name[1]', 'VARCHAR(80)'),
						CAST(REPLACE(X.value('Qty[1]', 'VARCHAR(19)'), ',', '.') AS DECIMAL(19,6)),
						CAST(REPLACE(X.value('Price[1]', 'VARCHAR(19)'), ',', '.') AS FLOAT),
						CAST(REPLACE(X.value('Rabat[1]', 'VARCHAR(19)'), ',', '.') AS FLOAT),
						--CONVERT(SMALLDATETIME, REPLACE(X.value('DeliveryDate[1]','VARCHAR(255)'), '. ', '.'), 104),
						X.value('Department[1]', 'CHAR(30)')
						--X.value('Note[1]', 'VARCHAR(80)')
					FROM @p_XMLOrder.nodes('/TransportOrder/Products/Product') AS X(X) 
				OPEN cPositions
				FETCH cPositions INTO @cIdent, @cName, @nQty, @nPrice, @nRebate, /*@dDeliveryDeadlinePos,*/ @cDeptPos--, @cNotePos
				WHILE @@fetch_status = 0
				BEGIN
					IF @cDeptPos IS NULL 
						SET @cDeptPos = ''
			
					SET @dDeliveryDeadlinePos = CAST(CAST(@dDeliveryDeadlinePos AS DATE) AS SMALLDATETIME)
			
					/*Kontrola podatkov.*/
					IF (SELECT COUNT(*) FROM tHE_SetSubj WHERE CAST(acSubject AS VARCHAR(30)) = @cDeptPos AND acDept = 'T') = 0 AND LTRIM(RTRIM(@cDeptPos)) <> ''
						SET @p_cError = 'Oddelek ' + LTRIM(RTRIM(ISNULL(@cDeptPos, ''))) + ' ne obstaja v šifrantu!'
					IF ISNULL(@nQty, 0) = 0
						SET @p_cError = 'Kolièina mora biti doloèena!'
					IF LTRIM(RTRIM(ISNULL(@cIdent, ''))) = ''
						SET @p_cError = 'Ident ne sme biti prazen!'
					IF (SELECT COUNT(*) FROM tHE_SetItem WHERE acIdent = @cIdent) = 0
						SET @p_cError = 'Ident ' + LTRIM(RTRIM(ISNULL(@cIdent, ''))) + ' ne obstaja v šifrantu identov!'
			
					IF ISNULL(@p_cError, '') <> ''
						RAISERROR(@p_cError, 16, 1)
			
					/*Kreiranje pozicije.*/
					EXEC pHE_OrderCreItemAll @p_cKey, @cIdent, @nQty, '', @nClerk, @nNo OUTPUT
					IF NOT EXISTS(SELECT acKey FROM tHE_OrderItem WHERE acKey = @p_cKey AND anNo = @nNo)
					BEGIN
						SET @p_cError = 'Napaka pri kreiranju pozicije naroèila!'
						RAISERROR(@p_cError, 16, 1)
					END
		
						UPDATE tHE_OrderItem
						SET acName = @cName,
							anPrice = @nPrice,
							anRebate1 = @nRebate,
							anRebate = @nRebate, 
							acDept = @cDeptPos
							/*acNote = @cNotePos,
							adDeliveryDeadline = ISNULL(NULLIF(@dDeliveryDeadlinePos, '19000101'), @dDeliveryDeadline)*/
						WHERE acKey = @p_cKey
							AND anNo = @nNo
		
					/*Preraèun pozicije.*/
					EXEC pHE_OrderItemInPVItem @p_cKey, @nNo
	 
					FETCH cPositions INTO @cIdent, @cName, @nQty, @nPrice, @nRebate, /*@dDeliveryDeadlinePos,*/ @cDeptPos--, @cNotePos
				END
				CLOSE cPositions
				DEALLOCATE cPositions

			
				/*Preraèun glave.*/
				EXEC pHE_OrderHeadSetSum 'P', 'EUR', 'EUR', 0.01, 0.01, @p_cKey		
		
				UPDATE tHE_Order
				SET  _acReport = @cReport
				WHERE acKey = @p_cKey	

			END
		END
		--------------------------------------------------------------------------------------------------------------------------------------

		if @p_XMLInvoice.exist('/ConnectedInvoices') > 0
		BEGIN

			/*Pridobivanje parametrov fakture.*/
			SELECT TOP 1 
				@cOrderNoFac = X.value('OrderNo[1]', 'VARCHAR(20)')
			FROM @p_XMLInvoice.nodes('/ConnectedInvoices') AS X(X) 

			SET @cOrderNoFacKey = left(ltrim(rtrim(@cOrderNoFac)),2) + Substring(ltrim(rtrim(@cOrderNoFac)),4,3) +'00'+right('00000'+ltrim(rtrim(@cOrderNoFac)),6)

			-- Èe ne obstaja OrderNo se uredijo povezve s faturami za to naroèilo
			IF isnull(@cOrderNoFac,'') <> '' and exists (select 1 from the_order where ackey = @cOrderNoFacKey)
			BEGIN

				/*Osveževanje povezav.*/
				DECLARE cPositionsFac CURSOR LOCAL FAST_FORWARD FOR
					SELECT 
						X.value('Action[1]', 'CHAR(16)'),
						X.value('Key[1]', 'VARCHAR(80)')					
					FROM @p_XMLInvoice.nodes('/ConnectedInvoices/Invoices/Invoice') AS X(X) 
					where X.value('Action[1]', 'CHAR(16)') in  ('ADD','DELETE')
				OPEN cPositionsFac
				FETCH cPositionsFac INTO @cAction, @cFacKey 
				WHILE @@fetch_status = 0
				BEGIN
				
					/*Brisanje povezave fakture z naroèilom*/
					--select @cOrderNoFacKey,@cAction, @cFacKey
					if @cAction = 'DELETE'
					begin
						delete from _epos_GTLink where NarocKljuc = @cOrderNoFacKey and LinkKljuc = @cFacKey
					end

					/*Dodajanje povezave fakture z naroèilom*/
					if @cAction = 'ADD' and not exists (select 1 from _epos_GTLink where NarocKljuc = @cOrderNoFacKey and LinkKljuc = @cFacKey)
					begin
						insert into _epos_GTLink(NarocKljuc, LinkKljuc)
						select @cOrderNoFacKey, @cFacKey
					end
				
					FETCH cPositionsFac INTO @cAction, @cFacKey
				END
				CLOSE cPositionsFac
				DEALLOCATE cPositionsFac

			END
		END

		IF @@TRANCOUNT > 0 AND @cTranStart = 'T'
			COMMIT TRANSACTION _upJM_CreateSuppOrderAndLink 
    END TRY
    BEGIN CATCH
		IF @@TRANCOUNT > 0 AND @cTranStart = 'T'
			ROLLBACK TRANSACTION _upJM_CreateSuppOrderAndLink

		DECLARE 
			@cMessage NVARCHAR(4000) = ERROR_MESSAGE(),
			@nSeverity INT = ERROR_SEVERITY(),
			@nState INT = ERROR_STATE()

		SET @p_cKey = NULL
		SET @p_cError = @cMessage --RAISERROR (@cMessage, @nSeverity, @nState)

		/*Pobris odprtih kurzorjev.*/
		IF (SELECT CURSOR_STATUS('global','cPositions')) >= -1
        BEGIN
            IF (SELECT CURSOR_STATUS('global','cPositions')) > -1
                CLOSE cPositions
            DEALLOCATE cPositions
        END
	END CATCH
END

GO


