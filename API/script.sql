USE [authApitest]
GO
/****** Object:  Table [dbo].[roles]    Script Date: 27-04-2022 01:32:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[roles](
	[roleId] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[roleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserMaster]    Script Date: 27-04-2022 01:32:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserMaster](
	[id] [int] IDENTITY(1000,1) NOT NULL,
	[FirstName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[Email] [varchar](100) NULL,
	[MobileNo] [varchar](100) NULL,
	[PasswordHash] [varchar](500) NULL,
	[DateCreate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[userRoles]    Script Date: 27-04-2022 01:32:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userRoles](
	[userId] [int] NOT NULL,
	[roleId] [int] NOT NULL,
 CONSTRAINT [PK_User_Roles] PRIMARY KEY CLUSTERED 
(
	[userId] ASC,
	[roleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_ReversePK] UNIQUE NONCLUSTERED 
(
	[roleId] ASC,
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[UserMaster] ADD  DEFAULT (getdate()) FOR [DateCreate]
GO
ALTER TABLE [dbo].[userRoles]  WITH CHECK ADD FOREIGN KEY([roleId])
REFERENCES [dbo].[roles] ([roleId])
GO
ALTER TABLE [dbo].[userRoles]  WITH CHECK ADD FOREIGN KEY([userId])
REFERENCES [dbo].[UserMaster] ([id])
GO
