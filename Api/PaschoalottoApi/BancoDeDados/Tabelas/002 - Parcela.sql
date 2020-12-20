IF NOT EXISTS( SELECT name
                 FROM SysObjects
				WHERE name = 'Parcela'
				  AND type = 'U' )
  BEGIN

    CREATE TABLE [dbo].[Parcela]
	(    
       IdParcela                INT IDENTITY(1,1) NOT NULL,
       IdTitulo                 INT               NOT NULL,
	   Numero                   VARCHAR(3)        NOT NULL,
	   DataVencimento           DATETIME          NOT NULL,
	   Valor                    DECIMAL(15,4)     NOT NULL

       -- Chave primária
       CONSTRAINT PkParcela 
	      PRIMARY KEY CLUSTERED([IdParcela]),

       -- Chave Única (Numero)
       CONSTRAINT UnParcelaNumero
	       UNIQUE NONCLUSTERED ([IdTitulo], [Numero]),
       
       -- Restrição: Coluna [Numero] não pode ser vazia
       CONSTRAINT CkParcelaNumero CHECK ([Numero] <> ''),
       
       -- Restrição: Coluna [Valor] não pode ser vazia
       CONSTRAINT CkParcelaValor CHECK ([Valor] > 0),

       -- Foreign Key: Coluna [IdTitulo] referenciando [dbo].[Titulo][IdTitulo]
       CONSTRAINT FkParcelaTitulo FOREIGN KEY (IdTitulo) REFERENCES [dbo].[Titulo]([IdTitulo])
	   
	)

  END
GO


