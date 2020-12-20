IF EXISTS( SELECT name
             FROM SysObjects
			WHERE name = 'stp_TituloInsert' 
			  AND type = 'P' )
  BEGIN
    
	DROP PROCEDURE [dbo].[stp_TituloInsert]

  END
GO

CREATE PROCEDURE [dbo].[stp_TituloInsert]
                  @IdTitulo                 INT OUTPUT,
				  @Numero                   VARCHAR(6),
				  @DevedorNome              VARCHAR(80),
				  @DevedorCpf               VARCHAR(11),
				  @JurosPercentual          Decimal(9,4),
				  @MultaPercentual          Decimal(9,4),
				  @ValorOriginal            Decimal(9,4)

AS
  BEGIN
    SET NOCOUNT ON


            INSERT INTO dbo.Titulo
                        (
						  Numero,
						  DevedorNome,
						  DevedorCpf,
						  JurosPercentual,
						  MultaPercentual,
						  ValorOriginal
                        )
                 VALUES
                        (
						  @Numero,
						  @DevedorNome,
						  @DevedorCpf,
						  @JurosPercentual,
						  @MultaPercentual,
						  @ValorOriginal
                        )

			SET @IdTitulo = @@IDENTITY


    SET NOCOUNT OFF


  END
GO