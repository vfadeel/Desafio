IF NOT EXISTS( SELECT name
                 FROM SysObjects
				WHERE name = 'Titulo'
				  AND type = 'U' )
  BEGIN

    CREATE TABLE [dbo].[Titulo]
	(    
       IdTitulo                 INT IDENTITY(1,1) NOT NULL,
       Numero                   VARCHAR(6)        NOT NULL,
       DevedorNome              VARCHAR(80)       NOT NULL,
       DevedorCpf               VARCHAR(11)       NOT NULL,
       JurosPercentual          Decimal(9,4)      NOT NULL,
       MultaPercentual          Decimal(9,4)      NOT NULL,
	   ValorOriginal            Decimal(9,4)      NOT NULL,


       -- Chave primária
       CONSTRAINT PkTitulo 
	      PRIMARY KEY CLUSTERED([IdTitulo]),

       -- Chave Única (Numero)
       CONSTRAINT UnTituloNumero
	       UNIQUE NONCLUSTERED ([Numero]),
       
       -- Restrição: Coluna [Numero] não pode ser vazia
       CONSTRAINT CkTituloNumero CHECK ([Numero] <> ''),
       
       -- Restrição: Coluna [DevedorNome] não pode ser vazia
       CONSTRAINT CkTituloDevedorNome CHECK ([DevedorNome] <> ''),
	   
       -- Restrição: Coluna [DevedorCpf] não pode ser vazia
       CONSTRAINT CkTituloDevedorCpf CHECK ([DevedorCpf] <> ''),
	   
       -- Restrição: Coluna [JurosPercentual] não pode ser menor que zero
       CONSTRAINT CkTituloJurosPercentual CHECK ([JurosPercentual] >= 0),
	   
       -- Restrição: Coluna [MultaPercentual] não pode ser menor que zero
       CONSTRAINT CkTituloMultaPercentual CHECK ([MultaPercentual] >= 0),
	   
       -- Restrição: Coluna [ValorOriginal] não pode ser menor que zero
       CONSTRAINT CkTituloValorOriginal CHECK ([ValorOriginal] >= 0)
	   
	)

  END
GO

