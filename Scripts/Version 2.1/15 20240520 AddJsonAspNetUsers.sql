IF NOT EXISTS(SELECT TOP 1 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'attrs' AND lower(TABLE_NAME) = lower('AspNetUsers'))
BEGIN
	alter table dbo.AspNetUsers add attrs nvarchar(2000);
	alter table dbo.AspNetUsers add constraint aspnusers_attrs_default default '{}' for attrs;
END
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE WHERE COLUMN_NAME = 'attrs' AND lower(CONSTRAINT_NAME) = lower('ck_aspnusers_attrs_isjson'))
BEGIN
	alter table dbo.AspNetUsers add constraint ck_aspnusers_attrs_isjson check(isjson(attrs) > 0);
END
GO

update dbo.AspNetUsers
set attrs = '{}'
where attrs is null;
