5.通过Migration生成数据库

在vs中的“程序包管理器控制台”中输入如下两个命令
Add-Migration init（执行此命令项目生成一个目录（Migration））

Update-Database init 执行新增数据库


8.字段修改 执行迁移的命令 
Add-Migration updatedb
Update-Database updatedb