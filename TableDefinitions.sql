/****** Object:  Table [dbo].[ItemOrderMaps] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemOrderMaps](
	[OrderId] [uniqueidentifier] NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Items] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items](
	[ItemId] [uniqueidentifier] NOT NULL,
	[ItemName] [nvarchar](50) NULL,
	[AddedDateTime] [datetime] NOT NULL,
	[LastUpdatedDateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderId] [uniqueidentifier] NOT NULL,
	[OrderName] [nvarchar](50) NULL,
	[AddedDateTime] [datetime] NOT NULL,
	[LastUpdatedDateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Items] ADD  DEFAULT (newid()) FOR [ItemId]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (newid()) FOR [OrderId]
GO
ALTER TABLE [dbo].[ItemOrderMaps]  WITH CHECK ADD FOREIGN KEY([ItemId])
REFERENCES [dbo].[Items] ([ItemId])
GO
ALTER TABLE [dbo].[ItemOrderMaps]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
GO
USE [master]
GO
ALTER DATABASE [BGLOrdersDB] SET  READ_WRITE 
GO