USE [DBTaskMasterCore]
GO

/****** Object:  Table [dbo].[Requests]    Script Date: 15.09.2018 14:34:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Requests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Comment] [nvarchar](1024) NULL,
	[Performers] [nvarchar](256) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[EndDate] [datetime] NULL,
	[Deadline] [datetime] NOT NULL,
	[StartDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Requests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Requests]  WITH CHECK ADD  CONSTRAINT [FK_Requests_Requests] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Requests] ([Id])
GO

ALTER TABLE [dbo].[Requests] CHECK CONSTRAINT [FK_Requests_Requests]
GO