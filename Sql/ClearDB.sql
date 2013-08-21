USE [hallo]
ALTER TABLE [dbo].[Articles] DROP CONSTRAINT [FK_dbo.Articles_dbo.Images_FrontpageImage_Id];
ALTER TABLE [dbo].[Articles] DROP CONSTRAINT [FK_dbo.Articles_dbo.ArticleCategories_Category2_Id];
ALTER TABLE [dbo].[Articles] DROP CONSTRAINT [FK_dbo.Articles_dbo.ArticleCategories_Category_Id];
drop table hallo.[dbo].[__MigrationHistory];
drop table hallo.[dbo].[ArticleCategories];
drop table hallo.[dbo].[Images];
drop table hallo.[dbo].[Articles];
drop table hallo.[dbo].[Messages];
drop table hallo.[dbo].[Users];
DROP TABLE hallo.dbo.FrontpageLinks;
