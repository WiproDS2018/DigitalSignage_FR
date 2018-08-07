

GO
/****** Object:  Table [dbo].[AssignPlayerDisplay]    Script Date: 16-Mar-18 7:25:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssignPlayerDisplay](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlayerId] [int] NOT NULL,
	[DisplayId] [int] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_AssignPlayerDisplay] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Campaign]    Script Date: 16-Mar-18 7:25:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Campaign](
	[CampaignId] [int] IDENTITY(1,1) NOT NULL,
	[CampaignName] [nvarchar](50) NULL,
	[DisplayId] [int] NULL,
	[SceneId] [int] NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[StartTime] [time](7) NULL,
	[EndTime] [time](7) NULL,
	[Frequency] [nvarchar](50) NULL,
	[Published] [bit] NULL,
	[Status] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[Interval] [int] NULL,
	[AccountID] [int] NULL,
	[CreatedBy] [int] NULL,
	[DeviceGroup] [varchar](50) NULL,
	[Zone] [varchar](50) NULL,
	[DaysOfWeek] [varchar](50) NULL,
 CONSTRAINT [PK_Campaign] PRIMARY KEY CLUSTERED 
(
	[CampaignId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CampaignSceneMapping]    Script Date: 16-Mar-18 7:25:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CampaignSceneMapping](
	[CampSceneId] [int] IDENTITY(1,1) NOT NULL,
	[CampaignId] [int] NOT NULL,
	[SceneId] [int] NOT NULL,
 CONSTRAINT [PK_CampaignSceneMapping] PRIMARY KEY CLUSTERED 
(
	[CampSceneId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[DeviceContentHistory]    Script Date: 16-Mar-18 7:25:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceContentHistory](
	[TranId] [int] IDENTITY(1,1) NOT NULL,
	[DeviceId] [int] NOT NULL,
	[DisplayId] [int] NOT NULL,
	[SceneId] [int] NOT NULL,
	[CampaignId] [int] NOT NULL,
	[ContentUrl] [nvarchar](max) NULL,
	[ContentType] [nvarchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[StartTime] [time](7) NULL,
	[EndTime] [time](7) NULL,
	[Interval] [int] NOT NULL,
	[Status] [nvarchar](50) NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_DeviceContentHistory] PRIMARY KEY CLUSTERED 
(
	[TranId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[DeviceContentTracker]    Script Date: 16-Mar-18 7:25:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeviceContentTracker](
	[TrackerId] [int] IDENTITY(1,1) NOT NULL,
	[DeviceId] [int] NOT NULL,
	[PlayerSerialNo] [nvarchar](max) NULL,
	[DisplayId] [int] NOT NULL,
	[SceneId] [int] NOT NULL,
	[CampaignId] [int] NOT NULL,
	[ContentUrl] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NULL,
	[ContentType] [nvarchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[StartTime] [time](7) NULL,
	[EndTime] [time](7) NULL,
	[Frequency] [nvarchar](50) NULL,
	[Published] [bit] NULL,
	[Status] [nvarchar](50) NULL,
	[ShedulerId] [int] NULL,
	[MessageId] [int] NULL,
	[Interval] [int] NULL,
	[TemplateType] [varchar](50) NULL,
	[IconPosition] [varchar](50) NULL,
	[ActionTime] [datetime] NULL,
	[Action] [varchar](250) NULL,
	[Zone] [varchar](50) NULL,
	[DaysOfWeek] [varchar](50) NULL,
 CONSTRAINT [PK_DeviceContentTracker] PRIMARY KEY CLUSTERED 
(
	[TrackerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Display]    Script Date: 16-Mar-18 7:25:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Display](
	[DisplayId] [int] IDENTITY(1,1) NOT NULL,
	[DisplayName] [nvarchar](max) NOT NULL,
	[Location] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
	[AccountID] [int] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [PK_Display] PRIMARY KEY CLUSTERED 
(
	[DisplayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[FaceRecogSignage]    Script Date: 16-Mar-18 7:25:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FaceRecogSignage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AgeLowerLimit] [int] NULL,
	[AgeUpperLimit] [int] NULL,
	[Gender] [nvarchar](50) NULL,
	[Signage] [nvarchar](250) NULL,
	[Title] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[SceneType] [nvarchar](50) NULL,
	[Duration] [int] NULL,
 CONSTRAINT [PK_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Player]    Script Date: 16-Mar-18 7:25:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Player](
	[PlayerId] [int] IDENTITY(1,1) NOT NULL,
	[PlayerSerialNo] [nvarchar](max) NOT NULL,
	[PlayerName] [nvarchar](max) NULL,
	[IsActive] [bit] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[AccountID] [int] NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_Player] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[PlayerGroup]    Script Date: 16-Mar-18 7:25:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerGroup](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](50) NULL,
	[GroupDescription] [nvarchar](200) NULL,
	[AccountID] [int] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[PlayerGroupMapping]    Script Date: 16-Mar-18 7:25:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerGroupMapping](
	[SerialNo] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NOT NULL,
	[PlayerId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SerialNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Role]    Script Date: 16-Mar-18 7:25:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NULL
PRIMARY KEY CLUSTERED 
(	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Scene]    Script Date: 16-Mar-18 7:25:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Scene](
	[SceneId] [int] IDENTITY(1,1) NOT NULL,
	[SceneName] [nvarchar](50) NULL,
	[SceneUrl] [nvarchar](max) NULL,
	[SceneContent] [nvarchar](max) NULL,
	[Comments] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
	[IsPrimaryApproved] [bit] NULL,
	[SceneType] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[Approver] [int] NULL,
	[UpdatedTime] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[AccountID] [int] NULL,
	[CreatedBy] [int] NULL,
	[TemplateType] [varchar](50) NULL,
	[IconPosition] [varchar](50) NULL,
 CONSTRAINT [PK_Scene] PRIMARY KEY CLUSTERED 
(
	[SceneId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TestTableOne]    Script Date: 16-Mar-18 7:25:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestTableOne](
	[Id] [int] NOT NULL,
	[AgeUpperLimit] [int] NULL,
	[AgeLowerLimit] [int] NULL,
	[Gender] [nvarchar](50) NULL,
	[Signage] [nvarchar](250) NULL,
	[Title] [nvarchar](50) NULL,
	[SceneType] [nvarchar](50) NULL,
	[Duration] [int] NULL,
	[IsActive] [bit] NULL,
	[createdAt] [datetimeoffset](7) NOT NULL,
	[updatedAt] [datetimeoffset](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[User]    Script Date: 16-Mar-18 7:25:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[ConfirmPassword] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
	[Role] [nvarchar](50) NULL,
	[AccountID] [int] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  View [dbo].[DigitalSignageBIView]    Script Date: 16-Mar-18 7:25:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Create View SignageBIView As

	--Select DCT.CampaignId,C.CampaignName,DCT.DisplayId,D.DisplayName,P.PlayerId,P.PlayerName,DCT.DeviceId,DCT.SceneId,S.SceneName,DCT.[Status]

		--From Player P, Campaign C, Display D, Scene S, DeviceContentTracker DCT

		--Where DCT.DeviceId = P.PlayerId AND C.CampaignId = DCT.CampaignId AND D.DisplayId = DCT.DisplayID AND S.SceneId = DCT.SceneId
		--Inner Join DeviceContentTracker On ( DCT.DeviceId = P.PlayerId) AND (C.CampaignId = DCT.CampaignId)AND(D.DisplayId = DCT.DisplayID) AND (S.SceneId = DCT.SceneId)

		Create View [dbo].[DigitalSignageBIView] As
		Select DCT.CampaignId,DCT.DisplayId,DCT.DeviceId,DCT.SceneId,DCT.[Status],C.CampaignName,D.DisplayName,P.PlayerName,S.SceneName	
		From DeviceContentTracker DCT Inner Join Player P On DCT.DeviceId = P.PlayerId 
								Inner Join Campaign C On  C.CampaignId = DCT.CampaignId
								Inner Join Display D On D.DisplayId = DCT.DisplayID
								Inner Join Scene S On S.SceneId = DCT.SceneId
GO
/****** Object:  View [dbo].[SignageBIView]    Script Date: 16-Mar-18 7:25:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[SignageBIView] As

	Select DCT.CampaignId,C.CampaignName,DCT.DisplayId,D.DisplayName,P.PlayerId,P.PlayerName,DCT.DeviceId,DCT.SceneId,S.SceneName,DCT.[Status]

		From Player P, Campaign C, Display D, Scene S, DeviceContentTracker DCT

		Where DCT.DeviceId = P.PlayerId AND C.CampaignId = DCT.CampaignId AND D.DisplayId = DCT.DisplayID AND S.SceneId = DCT.SceneId
GO
/****** Object:  View [dbo].[VWCampaignHistory]    Script Date: 16-Mar-18 7:25:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[VWCampaignHistory] AS

SELECT A.[TranId]
      ,A.[DeviceId]
      ,A.[DisplayId]
      ,A.[SceneId]
      ,A.[CampaignId]
      ,A.[ContentUrl]
      ,A.[ContentType]
      ,A.[StartDate]
      ,A.[EndDate]
      ,A.[StartTime]
      ,A.[EndTime]
      ,A.[Interval]
      ,A.[Status]
	  ,S.SceneName
	  ,P.Playername
	  ,P.PlayerSerialNo
	  ,C.CampaignName
	  ,D.DisplayName
      FROM DeviceContentHistory A
	   INNER JOIN Scene S
	  ON S.SceneId=A.SceneId
	  INNER JOIN Player P
	  ON P.PlayerId=A.DeviceId
	   INNER JOIN Campaign C
	  ON C.CampaignId=A.CampaignId
	   INNER JOIN  Display D
	  ON D.DisplayId=A.DisplayId
	 
	 


GO

GO
/****** Object:  StoredProcedure [dbo].[SP_AddCampHistory]    Script Date: 16-Mar-18 7:25:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_AddCampHistory]
 @CampaignId int,
 @Status nvarchar(50),
 @UserId int
 AS
 
BEGIN
	SET NOCOUNT ON;
	DECLARE @TmpTracker Table
       (
	        [DeviceId] [int],
			[PlayerSerialNo] [nvarchar](max) NULL,
			[DisplayId] [int],
			[SceneId] [int],
			[CampaignId] [int],
			[ContentUrl] [nvarchar](max),
			[Content] [nvarchar](max),
			[ContentType] [nvarchar](50),
			[StartDate] [datetime],
			[EndDate] [datetime],
			[StartTime] [time](7),
			[EndTime] [time](7),
			[Frequency] [nvarchar](50) ,
			[Published] [bit] ,
			[Status] [nvarchar](50)	,
			[Interval] [int]
	   )

INSERT INTO @TmpTracker
SELECT  A.[PlayerId]
		,P.[PlayerSerialNo]
		,C.[DisplayId]
		,MapScn.SceneId		
		,C.[CampaignId]
		,S.SceneUrl
		,S.SceneContent	 
		,SceneType 
        ,C.[StartDate]
        ,C.[EndDate]
        ,C.[StartTime]
        ,C.[EndTime]
        ,C.[Frequency]
        ,C.[Published]	
		,@Status as [Status] 
		,C.[Interval]		
	  FROM  Campaign C
	  INNER JOIN AssignPlayerDisplay A
	  ON A.DisplayId=C.DisplayId
	  INNER JOIN CampaignSceneMapping MapScn
	  ON MapScn.CampaignId= C.[CampaignId]
	  INNER JOIN Scene S
	  ON S.SceneId=MapScn.SceneId
	  INNER JOIN Player P
	  ON P.PlayerId=A.PlayerId
	  where C.[CampaignId]=@CampaignId

 INSERT INTO [DeviceContentHistory] ([DeviceId],[DisplayId],[SceneId],[CampaignId],[ContentUrl],[ContentType],[StartDate]
      ,[EndDate],[StartTime],[EndTime],[Interval],[Status],[UpdatedBy],[UpdatedTime] )
	   SELECT [DeviceId],[DisplayId],[SceneId],[CampaignId],[ContentUrl],[ContentType],[StartDate],[EndDate],
	   [StartTime],[EndTime],[Interval],[Status],@UserId as [UpdatedBy],getdate() as [UpdatedTime] FROM  @TmpTracker

	  -- select * from [DeviceContentHistory]

  
END




GO
/****** Object:  StoredProcedure [dbo].[SP_CampaignReport]    Script Date: 16-Mar-18 7:25:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CampaignReport]
 
 AS
 
BEGIN
	SET NOCOUNT ON;

SELECT A.[TranId]
      ,A.[DeviceId]
      ,A.[DisplayId]
      ,A.[SceneId]
      ,A.[CampaignId]
      ,A.[ContentUrl]
      ,A.[ContentType]
      ,A.[StartDate]
      ,A.[EndDate]
      ,A.[StartTime]
      ,A.[EndTime]
      ,A.[Interval]
      ,A.[Status]
	  ,S.SceneName
	  ,P.Playername
	  ,P.PlayerSerialNo
	  ,C.CampaignName
      FROM DeviceContentHistory A
	   INNER JOIN Scene S
	  ON S.SceneId=A.SceneId
	  INNER JOIN Player P
	  ON P.PlayerId=A.DeviceId
	   INNER JOIN Campaign C
	  ON C.CampaignId=A.CampaignId
END


GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateCampaignTime]    Script Date: 16-Mar-18 7:25:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 CREATE PROCEDURE [dbo].[SP_UpdateCampaignTime]
 @CampaignId int,
 @StartTime varchar(50),
 @EndTime varchar(50)
 AS
 
BEGIN
SET NOCOUNT ON;
UPDATE [Campaign] SET  [StartTime]=@StartTime,[EndTime]=@EndTime WHERE  [CampaignId]=@CampaignId
END

GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateDeviceTracker]    Script Date: 16-Mar-18 7:25:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_UpdateDeviceTracker]
 @CampaignId int,
 @Status nvarchar(50)
 AS
 
BEGIN
	SET NOCOUNT ON;

	Declare @DeviceGroup nvarchar(50)
	SELECT  @DeviceGroup=[DeviceGroup] FROM Campaign WHERE CampaignId=@CampaignId
	--SELECT @DeviceGroup

	DECLARE @TmpTracker Table
       (
	        [DeviceId] [int],
			[PlayerSerialNo] [nvarchar](max) NULL,
			[DisplayId] [int],
			[SceneId] [int],
			[CampaignId] [int],
			[ContentUrl] [nvarchar](max),
			[Content] [nvarchar](max),
			[ContentType] [nvarchar](50),
			[StartDate] [datetime],
			[EndDate] [datetime],
			[StartTime] [time](7),
			[EndTime] [time](7),
			[Frequency] [nvarchar](50) ,
			[Published] [bit] ,
			[Status] [nvarchar](50)	,
			[Interval] [int],
			[TemplateType] [varchar](50) ,
			[IconPosition] [varchar](50),
			[Zone] [varchar](50) ,
			[DaysOfWeek] [varchar](50)

	   )
If(@Status='Published')
Begin
------------Parent Group------------
If(@DeviceGroup='Parent Group')
 Begin
	INSERT INTO @TmpTracker
	SELECT  A.[PlayerId]
			,P.[PlayerSerialNo]
			,C.[DisplayId]
			,MapScn.SceneId		
			,C.[CampaignId]
			,S.SceneUrl
			,S.SceneContent	 
			,SceneType 
			,C.[StartDate]
			,C.[EndDate]
			,C.[StartTime]
			,C.[EndTime]
			,C.[Frequency]
			,C.[Published]	
			,'NotStarted' as [Status] 
			,C.[Interval]
			,S.[TemplateType]
			,S.[IconPosition]
			,C.[Zone]
			,C.[DaysOfWeek]	
		  FROM  Campaign C
		  INNER JOIN AssignPlayerDisplay A
		  ON A.DisplayId=C.DisplayId
		  INNER JOIN CampaignSceneMapping MapScn
		  ON MapScn.CampaignId= C.[CampaignId]
		  INNER JOIN Scene S
		  ON S.SceneId=MapScn.SceneId
		  INNER JOIN Player P
		  ON P.PlayerId=A.PlayerId
		  where C.[CampaignId]=@CampaignId
	 End 
	  ----------------End ParentGrp

	  ------------Sub Group------------
If(@DeviceGroup='Sub Group')
 Begin
	INSERT INTO @TmpTracker
	SELECT  A.[PlayerId]
			,P.[PlayerSerialNo]
			,C.[DisplayId]
			,MapScn.SceneId		
			,C.[CampaignId]
			,S.SceneUrl
			,S.SceneContent	 
			,SceneType 
			,C.[StartDate]
			,C.[EndDate]
			,C.[StartTime]
			,C.[EndTime]
			,C.[Frequency]
			,C.[Published]	
			,'NotStarted' as [Status] 
			,C.[Interval]
			,S.[TemplateType]
			,S.[IconPosition]
			,C.[Zone]
			,C.[DaysOfWeek]		
		  FROM  Campaign C
		  INNER JOIN PlayerGroupMapping A
		  ON A.GroupId=C.DisplayId
		  INNER JOIN CampaignSceneMapping MapScn
		  ON MapScn.CampaignId= C.[CampaignId]
		  INNER JOIN Scene S
		  ON S.SceneId=MapScn.SceneId
		  INNER JOIN Player P
		  ON P.PlayerId=A.PlayerId
		  where C.[CampaignId]=@CampaignId
	 End 
	  ----------------End SubGroup

	  ------------Device------------
If(@DeviceGroup='Devices')
 Begin
	INSERT INTO @TmpTracker
	SELECT  P.[PlayerId]
			,P.[PlayerSerialNo]
			,C.[DisplayId]
			,MapScn.SceneId		
			,C.[CampaignId]
			,S.SceneUrl
			,S.SceneContent	 
			,SceneType 
			,C.[StartDate]
			,C.[EndDate]
			,C.[StartTime]
			,C.[EndTime]
			,C.[Frequency]
			,C.[Published]	
			,'NotStarted' as [Status] 
			,C.[Interval]
			,S.[TemplateType]
			,S.[IconPosition]
			,C.[Zone]
			,C.[DaysOfWeek]		
		  FROM  Campaign C
		  INNER JOIN Player P
		  ON P.PlayerId=C.DisplayId
		  INNER JOIN CampaignSceneMapping MapScn
		  ON MapScn.CampaignId= C.[CampaignId]
		  INNER JOIN Scene S
		  ON S.SceneId=MapScn.SceneId		 
		  where C.[CampaignId]=@CampaignId
	 End 
	  ----------------End Device

	  Delete from [DeviceContentTracker] where CampaignId=@CampaignId

	INSERT INTO [DeviceContentTracker] ([DeviceId],[PlayerSerialNo],[DisplayId],[SceneId],[CampaignId],[ContentUrl],[Content],[ContentType],[StartDate]
      ,[EndDate],[StartTime],[EndTime],[Frequency],[Published],[Status],[Interval],[TemplateType],[IconPosition],[Zone] ,[DaysOfWeek])
	   SELECT [DeviceId],[PlayerSerialNo],[DisplayId],[SceneId],[CampaignId],[ContentUrl],[Content],
			[ContentType],[StartDate],[EndDate],[StartTime],[EndTime],[Frequency],[Published],[Status],[Interval],[TemplateType],[IconPosition],[Zone],[DaysOfWeek]  FROM  @TmpTracker
End 
----------------End Published

If(@Status='Canceled')
Begin
 UPDATE DeviceContentTracker SET Published=0,[Status]='NotStarted' WHERE CampaignId=@CampaignId
End	


  
END  --- End Procedure



GO



Create View [dbo].[VWDevicewithGroup] As

SELECT P.[PlayerId]
      ,P.[PlayerSerialNo]
      ,P.[PlayerName]
	  ,D.DisplayName as GroupName
	  ,D.[DisplayId] as GroupId
	  FROM [dbo].[Player] P
	   INNER JOIN AssignPlayerDisplay AP
	  ON P.[PlayerId]=AP.[PlayerId]
	   INNER JOIN Display D
	  ON D.[DisplayId]=AP.[DisplayId]
	  UNION
	  SELECT P.[PlayerId]
      ,P.[PlayerSerialNo]
      ,P.[PlayerName]
	  ,PGP.GroupNAme as GroupName
	  ,PGP.[GroupId] as GroupId
	  FROM [dbo].[Player] P
	  INNER JOIN PlayerGroupMapping PGM
	  ON P.[PlayerId]=PGM.[PlayerId]
	   INNER JOIN PlayerGroup PGP
	  ON PGP.[GroupId]=PGM.[GroupId]
	  UNION
 SELECT P.[PlayerId]
      ,P.[PlayerSerialNo]
      ,P.[PlayerName]
	  ,'Not Assigned' as GroupName
	  ,0  as GroupId
	   FROM [dbo].[Player] P
        LEFT JOIN AssignPlayerDisplay AP
	  ON P.[PlayerId]=AP.[PlayerId]
	    LEFT JOIN PlayerGroupMapping PGM
	  ON P.[PlayerId]=PGM.[PlayerId]
WHERE   AP.[PlayerId] IS NULL and PGM.[PlayerId] IS NULL 


GO

GO


CREATE PROCEDURE [dbo].[SP_GetCampaignList] 
 AS 
BEGIN
	SET NOCOUNT ON;
DECLARE @TmpTableCampaign TABLE
	(
	[CampaignId] [int]  NOT NULL,
	[CampaignName] [nvarchar](50) NULL,
	[DisplayId] [int] NULL,
	[GroupType] [nvarchar] (50),
	[GroupName] [nvarchar] (50),
	[SceneId] [int] NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[StartTime] [time](7) NULL,
	[EndTime] [time](7) NULL,
	[Frequency] [nvarchar](50) NULL,
	[Published] [bit] NULL,
	[Status] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[Interval] [int] NULL,
	[AccountID] [int] NULL,
	[CreatedBy] [int] NULL,
	[Zone] [varchar](50) NULL,
	[DaysOfWeek] [varchar](50) NULL,
	[SceneNames] VARCHAR(MAX) NULL,
	[MultiSceneIds] VARCHAR(MAX) NULL
	)
INSERT INTO @TmpTableCampaign
SELECT C.[CampaignId]
	  ,C.[CampaignName]
      ,C.[DisplayId]
	  ,C.[DeviceGroup] as GroupType
	  ,D.Displayname as GroupName
      ,C.[SceneId]
      ,C.[StartDate]
      ,C.[EndDate]
      ,C.[StartTime]
      ,C.[EndTime]
      ,C.[Frequency]
      ,C.[Published]
      ,C.[Status]
      ,C.[IsActive]
      ,C.[UpdatedBy]
      ,C.[UpdatedDate]
      ,C.[Interval]
      ,C.[AccountID]
      ,C.[CreatedBy]      
      ,C.[Zone]
      ,C.[DaysOfWeek]
	  ,'' as [SceneNames]
	  ,''as [MultiSceneIds]  
  FROM [dbo].[Campaign] C
	   INNER JOIN Display D
	  ON C.[DisplayId]=D.[DisplayId]
	   WHERE C.[DeviceGroup]='Parent Group'
	  
	  UNION
 SELECT C.[CampaignId]
      ,C.[CampaignName]
      ,C.[DisplayId]
	  ,C.[DeviceGroup] as GroupType
	  ,Pl.GroupName as GroupName
      ,C.[SceneId]
      ,C.[StartDate]
      ,C.[EndDate]
      ,C.[StartTime]
      ,C.[EndTime]
      ,C.[Frequency]
      ,C.[Published]
      ,C.[Status]
      ,C.[IsActive]
      ,C.[UpdatedBy]
      ,C.[UpdatedDate]
      ,C.[Interval]
      ,C.[AccountID]
      ,C.[CreatedBy]      
      ,C.[Zone]
      ,C.[DaysOfWeek]
	  ,'' as [SceneNames]
	  ,''as [MultiSceneIds]  	  
  FROM [dbo].[Campaign] C
	   INNER JOIN PlayerGroup Pl
	  ON C.[DisplayId]=Pl.[GroupId]
	   WHERE C.[DeviceGroup]='Sub Group'
	    UNION
 SELECT C.[CampaignId]
      ,C.[CampaignName]
      ,C.[DisplayId]
	  ,C.[DeviceGroup] as GroupType
	  ,P.PlayerName as GroupName
      ,C.[SceneId]
      ,C.[StartDate]
      ,C.[EndDate]
      ,C.[StartTime]
      ,C.[EndTime]
      ,C.[Frequency]
      ,C.[Published]
      ,C.[Status]
      ,C.[IsActive]
      ,C.[UpdatedBy]
      ,C.[UpdatedDate]
      ,C.[Interval]
      ,C.[AccountID]
      ,C.[CreatedBy]      
      ,C.[Zone]
      ,C.[DaysOfWeek]
	  ,'' as [SceneNames]
	  ,''as [MultiSceneIds]  	  
  FROM [dbo].[Campaign] C
	   INNER JOIN Player P
	  ON C.[DisplayId]=P.[PlayerId]
	   WHERE C.[DeviceGroup]='Devices'

 --SELECT * FROM @TmpTableCampaign

--Select *   FROM [dbo].[Campaign]

DECLARE @TmpTableCampaignIds TABLE
	(
	   [CampaignId]    INT   	
     , [CampaignName]  Varchar (250)    
	 , [Processed]      TINYINT Default 0
	)

	INSERT INTO @TmpTableCampaignIds
	SELECT CampaignId,CampaignName,0 FROM @TmpTableCampaign

	
	DECLARE  @Id INT 
	DECLARE @Scenes VARCHAR(MAX)
	DECLARE @MultisceneIds VARCHAR(MAX)    


WHILE (SELECT Count(*) From @TmpTableCampaignIds Where Processed = 0) > 0
    Begin
       SELECT TOP 1 @Id = CampaignId FROM @TmpTableCampaignIds  WHERE Processed = 0
			 
		 SET @Scenes =null
		 SET @MultisceneIds=null
		SELECT @Scenes = COALESCE(@Scenes + ', ', '') +  S.SceneName from Scene S  INNER JOIN  CampaignSceneMapping CMP  on CMP.SceneId=S.SceneId where CMP.CampaignId in (@Id)
		SELECT @MultisceneIds = COALESCE(@MultisceneIds + ',', '') + CAST(S.SceneId as varchar(10)) from Scene S  INNER JOIN  CampaignSceneMapping CMP  on CMP.SceneId=S.SceneId where CMP.CampaignId in (@Id)
		
		UPDATE @TmpTableCampaign set [SceneNames]=@Scenes,[MultiSceneIds]=@MultisceneIds  Where CampaignId = @Id 
		--SELECT @Scenes +':'+ @MultisceneIds AS Scenes		
		--SELECT  @Id  as CampId
	   	
       UPDATE @TmpTableCampaignIds Set Processed = 1 Where CampaignId = @Id 
    End
	  SELECT CampaignId,CampaignName, DisplayId,GroupName as DisplayName,SceneId,SceneNames as SceneName,StartDate,EndDate,StartTime,EndTime,'' as StartTimeVal,
        '' as EndTimeVal,IsActive, [Status],Frequency,Published as IsPublished,0 as UpdatedBy,getdate() as UpdatedTime,CAST(CONCAT([StartDate], ' ', [StartTime]) AS varchar(19)) as StartDateAndTime, CAST(CONCAT([EndDate], ' ', [EndTime]) AS varchar(19)) as EndDateAndTime,
        Interval,'' as OffsetTime,Zone,MultiSceneIds,'' as Copy,AccountID,CreatedBy,Grouptype as[Type],DaysOfWeek FROM @TmpTableCampaign
END

GO
