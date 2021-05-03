USE [master]
GO
/****** You Can change your database name or skip creating database ******/
CREATE DATABASE [WorkSchedule]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WorkSchedule', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\WorkSchedule.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WorkSchedule_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\WorkSchedule_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [WorkSchedule] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WorkSchedule].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WorkSchedule] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WorkSchedule] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WorkSchedule] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WorkSchedule] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WorkSchedule] SET ARITHABORT OFF 
GO
ALTER DATABASE [WorkSchedule] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WorkSchedule] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WorkSchedule] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WorkSchedule] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WorkSchedule] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WorkSchedule] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WorkSchedule] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WorkSchedule] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WorkSchedule] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WorkSchedule] SET  DISABLE_BROKER 
GO
ALTER DATABASE [WorkSchedule] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WorkSchedule] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WorkSchedule] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WorkSchedule] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WorkSchedule] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WorkSchedule] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WorkSchedule] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WorkSchedule] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [WorkSchedule] SET  MULTI_USER 
GO
ALTER DATABASE [WorkSchedule] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WorkSchedule] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WorkSchedule] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WorkSchedule] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WorkSchedule] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [WorkSchedule] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [WorkSchedule] SET QUERY_STORE = OFF
GO
USE [WorkSchedule]
GO
/****** Object:  Table [dbo].[Audits]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Audits](
	[AuditId] [int] IDENTITY(1,1) NOT NULL,
	[EntityName] [nvarchar](max) NULL,
	[PropertyName] [nvarchar](max) NULL,
	[PrimaryKeyValue] [nvarchar](max) NULL,
	[OldValue] [nvarchar](max) NULL,
	[NewValue] [nvarchar](max) NULL,
	[DateTimeStamp] [datetime] NOT NULL,
	[ChangedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Audits] PRIMARY KEY CLUSTERED 
(
	[AuditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DictDaysOffs]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DictDaysOffs](
	[DictDaysOffId] [int] IDENTITY(1,1) NOT NULL,
	[DictDepartmentID] [int] NOT NULL,
	[DateDayOff] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.DictDaysOffs] PRIMARY KEY CLUSTERED 
(
	[DictDaysOffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DictDepartments]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DictDepartments](
	[DictDepartmentID] [int] IDENTITY(1,1) NOT NULL,
	[DictDepartmentForeignID] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.DictDepartments] PRIMARY KEY CLUSTERED 
(
	[DictDepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DictJobTitles]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DictJobTitles](
	[DictJobTitleID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.DictJobTitles] PRIMARY KEY CLUSTERED 
(
	[DictJobTitleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DictRoles]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DictRoles](
	[DictRolesId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.DictRoles] PRIMARY KEY CLUSTERED 
(
	[DictRolesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DictScheduleStatus]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DictScheduleStatus](
	[DictScheduleStatusID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ColorHTML] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.DictScheduleStatus] PRIMARY KEY CLUSTERED 
(
	[DictScheduleStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DictSkills]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DictSkills](
	[DictSkillID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.DictSkills] PRIMARY KEY CLUSTERED 
(
	[DictSkillID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DictTeams]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DictTeams](
	[DictTeamID] [int] IDENTITY(1,1) NOT NULL,
	[DictTeamForeignID] [int] NOT NULL,
	[DictDepartmentID] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.DictTeams] PRIMARY KEY CLUSTERED 
(
	[DictTeamID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[People]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[People](
	[PersonId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Surname] [nvarchar](max) NULL,
	[FullName] [nvarchar](max) NULL,
	[Login] [nvarchar](max) NOT NULL,
	[active] [bit] NOT NULL,
	[DictDepartmentID] [int] NOT NULL,
	[DictTeamID] [int] NULL,
	[FormOfEmployment] [nvarchar](max) NULL,
	[TypeOfEmployment] [decimal](18, 2) NOT NULL,
	[DictJobTitleID] [int] NOT NULL,
	[DictSkillID] [int] NOT NULL,
	[DateFrom] [datetime] NOT NULL,
	[DateTo] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.People] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonInRoles]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonInRoles](
	[PersonInRolesId] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[DictRolesId] [int] NOT NULL,
	[DateFrom] [datetime] NOT NULL,
	[DateTo] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.PersonInRoles] PRIMARY KEY CLUSTERED 
(
	[PersonInRolesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonSkills]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonSkills](
	[PersonSkillsId] [int] IDENTITY(1,1) NOT NULL,
	[DictSkillID] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[PersonId] [int] NOT NULL,
	[DateFrom] [datetime] NOT NULL,
	[DateTo] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.PersonSkills] PRIMARY KEY CLUSTERED 
(
	[PersonSkillsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportSaveds]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportSaveds](
	[ReportSavedId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[State] [nvarchar](max) NULL,
	[global] [bit] NOT NULL,
	[PersonId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ReportSaveds] PRIMARY KEY CLUSTERED 
(
	[ReportSavedId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ScheduleExtraAdds]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleExtraAdds](
	[ScheduleExtraAddsId] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[DateStart] [datetime] NOT NULL,
	[DateEnd] [datetime] NOT NULL,
	[ScheduleExtraAddsStatus] [nvarchar](max) NULL,
	[ScheduleExtraAddsDesc] [nvarchar](max) NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.ScheduleExtraAdds] PRIMARY KEY CLUSTERED 
(
	[ScheduleExtraAddsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schedules]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedules](
	[ScheduleId] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[DateStart] [datetime] NOT NULL,
	[DateEnd] [datetime] NOT NULL,
	[ScheduleStatus] [nvarchar](max) NOT NULL,
	[ScheduleDesc] [nvarchar](max) NULL,
	[WorkplaceId] [int] NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Schedules] PRIMARY KEY CLUSTERED 
(
	[ScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchedulesOverlappings]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchedulesOverlappings](
	[ScheduleId] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[DateStart] [datetime] NOT NULL,
	[DateEnd] [datetime] NOT NULL,
	[ScheduleStatus] [nvarchar](max) NOT NULL,
	[ScheduleDesc] [nvarchar](max) NULL,
	[WorkplaceId] [int] NULL,
	[active] [bit] NOT NULL,
	[ScheduleStatus2] [nvarchar](max) NULL,
	[secondsoverlap] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SchedulesOverlappings] PRIMARY KEY CLUSTERED 
(
	[ScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Workplaces]    Script Date: 03.05.2021 16:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Workplaces](
	[WorkplaceId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[DictDepartmentID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Workplaces] PRIMARY KEY CLUSTERED 
(
	[WorkplaceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[DictDepartments] ON 
GO
INSERT [dbo].[DictDepartments] ([DictDepartmentID], [DictDepartmentForeignID], [Name]) VALUES (1, 599, N'Department FO')
GO
INSERT [dbo].[DictDepartments] ([DictDepartmentID], [DictDepartmentForeignID], [Name]) VALUES (2, 598, N'Department BO')
GO
SET IDENTITY_INSERT [dbo].[DictDepartments] OFF
GO
SET IDENTITY_INSERT [dbo].[DictRoles] ON 
GO
INSERT [dbo].[DictRoles] ([DictRolesId], [Name]) VALUES (1, N'Admin')
GO
INSERT [dbo].[DictRoles] ([DictRolesId], [Name]) VALUES (2, N'User')
GO
INSERT [dbo].[DictRoles] ([DictRolesId], [Name]) VALUES (3, N'Moderator')
GO
SET IDENTITY_INSERT [dbo].[DictRoles] OFF
GO
/****** Object:  Index [IX_DictDepartmentID]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_DictDepartmentID] ON [dbo].[DictDaysOffs]
(
	[DictDepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DictDepartmentID]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_DictDepartmentID] ON [dbo].[DictTeams]
(
	[DictDepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DictDepartmentID]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_DictDepartmentID] ON [dbo].[People]
(
	[DictDepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DictJobTitleID]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_DictJobTitleID] ON [dbo].[People]
(
	[DictJobTitleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DictSkillID]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_DictSkillID] ON [dbo].[People]
(
	[DictSkillID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DictTeamID]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_DictTeamID] ON [dbo].[People]
(
	[DictTeamID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DictRolesId]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_DictRolesId] ON [dbo].[PersonInRoles]
(
	[DictRolesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PersonId]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_PersonId] ON [dbo].[PersonInRoles]
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PersonId]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_PersonId] ON [dbo].[PersonSkills]
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PersonId]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_PersonId] ON [dbo].[ReportSaveds]
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PersonId]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_PersonId] ON [dbo].[Schedules]
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkplaceId]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_WorkplaceId] ON [dbo].[Schedules]
(
	[WorkplaceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PersonId]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_PersonId] ON [dbo].[SchedulesOverlappings]
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkplaceId]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_WorkplaceId] ON [dbo].[SchedulesOverlappings]
(
	[WorkplaceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DictDepartmentID]    Script Date: 03.05.2021 16:55:38 ******/
CREATE NONCLUSTERED INDEX [IX_DictDepartmentID] ON [dbo].[Workplaces]
(
	[DictDepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DictDaysOffs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.DictDaysOffs_dbo.DictDepartments_DictDepartmentID] FOREIGN KEY([DictDepartmentID])
REFERENCES [dbo].[DictDepartments] ([DictDepartmentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DictDaysOffs] CHECK CONSTRAINT [FK_dbo.DictDaysOffs_dbo.DictDepartments_DictDepartmentID]
GO
ALTER TABLE [dbo].[DictTeams]  WITH CHECK ADD  CONSTRAINT [FK_dbo.DictTeams_dbo.DictDepartments_DictDepartmentID] FOREIGN KEY([DictDepartmentID])
REFERENCES [dbo].[DictDepartments] ([DictDepartmentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DictTeams] CHECK CONSTRAINT [FK_dbo.DictTeams_dbo.DictDepartments_DictDepartmentID]
GO
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_dbo.People_dbo.DictDepartments_DictDepartmentID] FOREIGN KEY([DictDepartmentID])
REFERENCES [dbo].[DictDepartments] ([DictDepartmentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_dbo.People_dbo.DictDepartments_DictDepartmentID]
GO
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_dbo.People_dbo.DictJobTitles_DictJobTitleID] FOREIGN KEY([DictJobTitleID])
REFERENCES [dbo].[DictJobTitles] ([DictJobTitleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_dbo.People_dbo.DictJobTitles_DictJobTitleID]
GO
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_dbo.People_dbo.DictSkills_DictSkillID] FOREIGN KEY([DictSkillID])
REFERENCES [dbo].[DictSkills] ([DictSkillID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_dbo.People_dbo.DictSkills_DictSkillID]
GO
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_dbo.People_dbo.DictTeams_DictTeamID] FOREIGN KEY([DictTeamID])
REFERENCES [dbo].[DictTeams] ([DictTeamID])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_dbo.People_dbo.DictTeams_DictTeamID]
GO
ALTER TABLE [dbo].[PersonInRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PersonInRoles_dbo.DictRoles_DictRolesId] FOREIGN KEY([DictRolesId])
REFERENCES [dbo].[DictRoles] ([DictRolesId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PersonInRoles] CHECK CONSTRAINT [FK_dbo.PersonInRoles_dbo.DictRoles_DictRolesId]
GO
ALTER TABLE [dbo].[PersonInRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PersonInRoles_dbo.People_PersonId] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PersonInRoles] CHECK CONSTRAINT [FK_dbo.PersonInRoles_dbo.People_PersonId]
GO
ALTER TABLE [dbo].[PersonSkills]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PersonSkills_dbo.People_PersonId] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PersonSkills] CHECK CONSTRAINT [FK_dbo.PersonSkills_dbo.People_PersonId]
GO
ALTER TABLE [dbo].[ReportSaveds]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReportSaveds_dbo.People_PersonId] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ReportSaveds] CHECK CONSTRAINT [FK_dbo.ReportSaveds_dbo.People_PersonId]
GO
ALTER TABLE [dbo].[Schedules]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Schedules_dbo.People_PersonId] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Schedules] CHECK CONSTRAINT [FK_dbo.Schedules_dbo.People_PersonId]
GO
ALTER TABLE [dbo].[Schedules]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Schedules_dbo.Workplaces_WorkplaceId] FOREIGN KEY([WorkplaceId])
REFERENCES [dbo].[Workplaces] ([WorkplaceId])
GO
ALTER TABLE [dbo].[Schedules] CHECK CONSTRAINT [FK_dbo.Schedules_dbo.Workplaces_WorkplaceId]
GO
ALTER TABLE [dbo].[SchedulesOverlappings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SchedulesOverlappings_dbo.People_PersonId] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SchedulesOverlappings] CHECK CONSTRAINT [FK_dbo.SchedulesOverlappings_dbo.People_PersonId]
GO
ALTER TABLE [dbo].[SchedulesOverlappings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SchedulesOverlappings_dbo.Workplaces_WorkplaceId] FOREIGN KEY([WorkplaceId])
REFERENCES [dbo].[Workplaces] ([WorkplaceId])
GO
ALTER TABLE [dbo].[SchedulesOverlappings] CHECK CONSTRAINT [FK_dbo.SchedulesOverlappings_dbo.Workplaces_WorkplaceId]
GO
ALTER TABLE [dbo].[Workplaces]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Workplaces_dbo.DictDepartments_DictDepartmentID] FOREIGN KEY([DictDepartmentID])
REFERENCES [dbo].[DictDepartments] ([DictDepartmentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Workplaces] CHECK CONSTRAINT [FK_dbo.Workplaces_dbo.DictDepartments_DictDepartmentID]
GO
USE [master]
GO
ALTER DATABASE [WorkSchedule] SET  READ_WRITE 
GO
