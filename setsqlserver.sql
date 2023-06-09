USE [master]
GO
/****** Object:  Database [sistema_banco]    Script Date: 30/05/2023 15:55:46 ******/
CREATE DATABASE [sistema_banco]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SistemaBanco', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\SistemaBanco.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SistemaBanco_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\SistemaBanco_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [sistema_banco] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [sistema_banco].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [sistema_banco] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [sistema_banco] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [sistema_banco] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [sistema_banco] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [sistema_banco] SET ARITHABORT OFF 
GO
ALTER DATABASE [sistema_banco] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [sistema_banco] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [sistema_banco] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [sistema_banco] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [sistema_banco] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [sistema_banco] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [sistema_banco] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [sistema_banco] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [sistema_banco] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [sistema_banco] SET  DISABLE_BROKER 
GO
ALTER DATABASE [sistema_banco] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [sistema_banco] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [sistema_banco] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [sistema_banco] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [sistema_banco] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [sistema_banco] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [sistema_banco] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [sistema_banco] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [sistema_banco] SET  MULTI_USER 
GO
ALTER DATABASE [sistema_banco] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [sistema_banco] SET DB_CHAINING OFF 
GO
ALTER DATABASE [sistema_banco] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [sistema_banco] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [sistema_banco] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [sistema_banco] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [sistema_banco] SET QUERY_STORE = ON
GO
ALTER DATABASE [sistema_banco] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [sistema_banco]
GO
/****** Object:  Table [dbo].[contas]    Script Date: 30/05/2023 15:55:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[contas](
	[codConta] [varchar](25) NOT NULL,
	[agencia] [varchar](6) NULL,
	[senha] [varbinary](100) NULL,
	[saldo] [money] NULL,
	[tipo] [tinyint] NOT NULL,
	[idUsuario] [int] NULL,
 CONSTRAINT [PK__contas__B89B358CE048E355] PRIMARY KEY CLUSTERED 
(
	[codConta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mov]    Script Date: 30/05/2023 15:55:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mov](
	[idMov] [int] IDENTITY(1,1) NOT NULL,
	[idConta] [varchar](25) NOT NULL,
	[dataHora] [varchar](20) NULL,
	[valor] [money] NOT NULL,
	[tipo] [varchar](20) NOT NULL,
 CONSTRAINT [PK__mov__3DC69A4F779BE56D] PRIMARY KEY CLUSTERED 
(
	[idMov] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[usuarios]    Script Date: 30/05/2023 15:55:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[usuarios](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nome] [varchar](100) NOT NULL,
 CONSTRAINT [PK__usuarios__3213E83F7722FE27] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[contas] ([codConta], [agencia], [senha], [saldo], [tipo], [idUsuario]) VALUES (N'123456', NULL, NULL, 10.0000, 0, 2)
INSERT [dbo].[contas] ([codConta], [agencia], [senha], [saldo], [tipo], [idUsuario]) VALUES (N'654321', NULL, NULL, 50.4000, 0, 3)
GO
SET IDENTITY_INSERT [dbo].[mov] ON 

INSERT [dbo].[mov] ([idMov], [idConta], [dataHora], [valor], [tipo]) VALUES (21, N'123456', N'16:17,26/05/23', 100.0000, N'transferencia')
INSERT [dbo].[mov] ([idMov], [idConta], [dataHora], [valor], [tipo]) VALUES (22, N'123456', N'16:18,26/05/23', 122.0000, N'deposito')
INSERT [dbo].[mov] ([idMov], [idConta], [dataHora], [valor], [tipo]) VALUES (23, N'123456', N'16:22,26/05/23', 100.0000, N'deposito')
INSERT [dbo].[mov] ([idMov], [idConta], [dataHora], [valor], [tipo]) VALUES (24, N'123456', N'16:22,26/05/23', 20.0000, N'transferencia')
INSERT [dbo].[mov] ([idMov], [idConta], [dataHora], [valor], [tipo]) VALUES (25, N'123456', N'13:35,30/05/23', 100.0000, N'deposito')
INSERT [dbo].[mov] ([idMov], [idConta], [dataHora], [valor], [tipo]) VALUES (26, N'123456', N'14:01,30/05/23', 100.0000, N'deposito')
INSERT [dbo].[mov] ([idMov], [idConta], [dataHora], [valor], [tipo]) VALUES (27, N'123456', N'14:01,30/05/23', 1.0000, N'saque')
INSERT [dbo].[mov] ([idMov], [idConta], [dataHora], [valor], [tipo]) VALUES (28, N'123456', N'14:02,30/05/23', 20.0000, N'transferencia')
INSERT [dbo].[mov] ([idMov], [idConta], [dataHora], [valor], [tipo]) VALUES (30, N'123456', N'14:40,30/05/23', 40.4500, N'deposito')
INSERT [dbo].[mov] ([idMov], [idConta], [dataHora], [valor], [tipo]) VALUES (31, N'123456', N'14:56,30/05/23', 100.4500, N'saque')
INSERT [dbo].[mov] ([idMov], [idConta], [dataHora], [valor], [tipo]) VALUES (32, N'123456', N'15:28,30/05/23', 10.0000, N'deposito')
INSERT [dbo].[mov] ([idMov], [idConta], [dataHora], [valor], [tipo]) VALUES (33, N'123456', N'15:29,30/05/23', 10.0000, N'deposito')
INSERT [dbo].[mov] ([idMov], [idConta], [dataHora], [valor], [tipo]) VALUES (34, N'123456', N'15:32,30/05/23', 10.0000, N'saque')
SET IDENTITY_INSERT [dbo].[mov] OFF
GO
SET IDENTITY_INSERT [dbo].[usuarios] ON 

INSERT [dbo].[usuarios] ([id], [nome]) VALUES (2, N'JHON')
INSERT [dbo].[usuarios] ([id], [nome]) VALUES (3, N'jao')
SET IDENTITY_INSERT [dbo].[usuarios] OFF
GO
ALTER TABLE [dbo].[contas] ADD  CONSTRAINT [DF__contas__saldo__4BAC3F29]  DEFAULT ((0)) FOR [saldo]
GO
ALTER TABLE [dbo].[contas]  WITH CHECK ADD  CONSTRAINT [FK__contas__idUsuari__4CA06362] FOREIGN KEY([idUsuario])
REFERENCES [dbo].[usuarios] ([id])
GO
ALTER TABLE [dbo].[contas] CHECK CONSTRAINT [FK__contas__idUsuari__4CA06362]
GO
ALTER TABLE [dbo].[mov]  WITH CHECK ADD  CONSTRAINT [FK__mov__idConta__571DF1D5] FOREIGN KEY([idConta])
REFERENCES [dbo].[contas] ([codConta])
GO
ALTER TABLE [dbo].[mov] CHECK CONSTRAINT [FK__mov__idConta__571DF1D5]
GO
USE [master]
GO
ALTER DATABASE [sistema_banco] SET  READ_WRITE 
GO
