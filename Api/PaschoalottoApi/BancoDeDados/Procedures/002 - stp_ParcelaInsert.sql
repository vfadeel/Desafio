IF EXISTS( SELECT name
             FROM SysObjects
			WHERE name = 'stp_ParcelaInsert' 
			  AND type = 'P' )
  BEGIN
    
	DROP PROCEDURE [dbo].[stp_ParcelaInsert]

  END
GO

CREATE PROCEDURE [dbo].[stp_ParcelaInsert]
                  @IdParcela              INT OUTPUT,
				  @IdTitulo               INT,
				  @Numero                 VARCHAR(3),
				  @DataVencimento         DATETIME,
				  @Valor                  DECIMAL(15,4)

AS
  BEGIN
    SET NOCOUNT ON


            INSERT INTO dbo.Parcela
                        (
						  IdTitulo,
						  Numero,
						  DataVencimento,
						  Valor
                        )
                 VALUES
                        (
						  @IdTitulo,
						  @Numero,
						  @DataVencimento,
						  @Valor
                        )

			SET @IdParcela = @@IDENTITY


    SET NOCOUNT OFF


  END
GO