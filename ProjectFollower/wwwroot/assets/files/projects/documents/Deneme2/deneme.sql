USE [QSEvrak2020]
GO

/****** Object:  Table [dbo].[evrakKaydet]    Script Date: 4.01.2022 10:27:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[evrakKaydet](
	[idEvrak] [int] NOT NULL,
	[idPersonel] [int] NOT NULL,
	[evrak] [varbinary](max) NULL,
	[doysaAdi] [varchar](50) NULL,
 CONSTRAINT [PK_evrakKaydet] PRIMARY KEY CLUSTERED 
(
	[idEvrak] ASC,
	[idPersonel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

