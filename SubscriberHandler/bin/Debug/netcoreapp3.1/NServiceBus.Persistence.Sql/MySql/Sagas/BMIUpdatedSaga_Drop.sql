
/* TableNameVariable */

set @tableNameQuoted = concat('`', @tablePrefix, 'BMIUpdatedSaga`');
set @tableNameNonQuoted = concat(@tablePrefix, 'BMIUpdatedSaga');


/* DropTable */

set @dropTable = concat('drop table if exists ', @tableNameQuoted);
prepare script from @dropTable;
execute script;
deallocate prepare script;
