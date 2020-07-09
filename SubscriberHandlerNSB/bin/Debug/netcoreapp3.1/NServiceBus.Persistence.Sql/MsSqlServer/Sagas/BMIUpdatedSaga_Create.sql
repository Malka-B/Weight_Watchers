
/* TableNameVariable */

declare @tableName nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + N'BMIUpdatedSaga]';
declare @tableNameWithoutSchema nvarchar(max) = @tablePrefix + N'BMIUpdatedSaga';


/* Initialize */

/* CreateTable */

if not exists
(
    select *
    from sys.objects
    where
        object_id = object_id(@tableName) and
        type in ('U')
)
begin
declare @createTable nvarchar(max);
set @createTable = '
    create table ' + @tableName + '(
        Id uniqueidentifier not null primary key,
        Metadata nvarchar(max) not null,
        Data nvarchar(max) not null,
        PersistenceVersion varchar(23) not null,
        SagaTypeVersion varchar(23) not null,
        Concurrency int not null
    )
';
exec(@createTable);
end

/* AddProperty MeasureId */

if not exists
(
  select * from sys.columns
  where
    name = N'Correlation_MeasureId' and
    object_id = object_id(@tableName)
)
begin
  declare @createColumn_MeasureId nvarchar(max);
  set @createColumn_MeasureId = '
  alter table ' + @tableName + N'
    add Correlation_MeasureId bigint;';
  exec(@createColumn_MeasureId);
end

/* VerifyColumnType Int */

declare @dataType_MeasureId nvarchar(max);
set @dataType_MeasureId = (
  select data_type
  from INFORMATION_SCHEMA.COLUMNS
  where
    table_name = @tableNameWithoutSchema and
    table_schema = @schema and
    column_name = 'Correlation_MeasureId'
);
if (@dataType_MeasureId <> 'bigint')
  begin
    declare @error_MeasureId nvarchar(max) = N'Incorrect data type for Correlation_MeasureId. Expected bigint got ' + @dataType_MeasureId + '.';
    throw 50000, @error_MeasureId, 0
  end

/* WriteCreateIndex MeasureId */

if not exists
(
    select *
    from sys.indexes
    where
        name = N'Index_Correlation_MeasureId' and
        object_id = object_id(@tableName)
)
begin
  declare @createIndex_MeasureId nvarchar(max);
  set @createIndex_MeasureId = N'
  create unique index Index_Correlation_MeasureId
  on ' + @tableName + N'(Correlation_MeasureId)
  where Correlation_MeasureId is not null;';
  exec(@createIndex_MeasureId);
end

/* PurgeObsoleteIndex */

declare @dropIndexQuery nvarchar(max);
select @dropIndexQuery =
(
    select 'drop index ' + name + ' on ' + @tableName + ';'
    from sysindexes
    where
        Id = object_id(@tableName) and
        Name is not null and
        Name like 'Index_Correlation_%' and
        Name <> N'Index_Correlation_MeasureId'
);
exec sp_executesql @dropIndexQuery

/* PurgeObsoleteProperties */

declare @dropPropertiesQuery nvarchar(max);
select @dropPropertiesQuery =
(
    select 'alter table ' + @tableName + ' drop column ' + column_name + ';'
    from INFORMATION_SCHEMA.COLUMNS
    where
        table_name = @tableNameWithoutSchema and
        table_schema = @schema and
        column_name like 'Correlation_%' and
        column_name <> N'Correlation_MeasureId'
);
exec sp_executesql @dropPropertiesQuery

/* CompleteSagaScript */
