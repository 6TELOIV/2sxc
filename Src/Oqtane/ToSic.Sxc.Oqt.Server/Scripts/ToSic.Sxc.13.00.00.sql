SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* TargetTypes Metadata for ..., Custom and Custom1-9 */
UPDATE [dbo].[ToSIC_EAV_AssignmentObjectTypes] SET [Name] = 'Zone' ,[Description] = 'Metadata for Zone' WHERE [AssignmentObjectTypeID] = 6
UPDATE [dbo].[ToSIC_EAV_AssignmentObjectTypes] SET [Name] = 'System' ,[Description] = 'Metadata for System' WHERE [AssignmentObjectTypeID] = 11
UPDATE [dbo].[ToSIC_EAV_AssignmentObjectTypes] SET [Name] = 'Site' ,[Description] = 'Metadata for Site' WHERE [AssignmentObjectTypeID] = 12
UPDATE [dbo].[ToSIC_EAV_AssignmentObjectTypes] SET [Name] = 'SiteVariant' ,[Description] = 'Metadata for SiteVariant' WHERE [AssignmentObjectTypeID] = 13
UPDATE [dbo].[ToSIC_EAV_AssignmentObjectTypes] SET [Name] = 'Page' ,[Description] = 'Metadata for Page' WHERE [AssignmentObjectTypeID] = 14
UPDATE [dbo].[ToSIC_EAV_AssignmentObjectTypes] SET [Name] = 'PageVariant' ,[Description] = 'Metadata for PageVariant' WHERE [AssignmentObjectTypeID] = 15
UPDATE [dbo].[ToSIC_EAV_AssignmentObjectTypes] SET [Name] = 'Module' ,[Description] = 'Metadata for Module' WHERE [AssignmentObjectTypeID] = 16
UPDATE [dbo].[ToSIC_EAV_AssignmentObjectTypes] SET [Name] = 'ModuleVariant' ,[Description] = 'Metadata for ModuleVariant' WHERE [AssignmentObjectTypeID] = 17
UPDATE [dbo].[ToSIC_EAV_AssignmentObjectTypes] SET [Name] = 'User' ,[Description] = 'Metadata for User' WHERE [AssignmentObjectTypeID] = 18
UPDATE [dbo].[ToSIC_EAV_AssignmentObjectTypes] SET [Name] = 'Custom' + IIF ([AssignmentObjectTypeID] = 90, '', LTRIM(STR([AssignmentObjectTypeID]-90))),
	[Description] = 'Custom' + IIF ([AssignmentObjectTypeID] = 90, '', LTRIM(STR([AssignmentObjectTypeID]-90)))
	WHERE [AssignmentObjectTypeID] > 89 AND [AssignmentObjectTypeID] < 100
GO