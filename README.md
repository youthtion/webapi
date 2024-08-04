# WEBAPI POST功能
1.檢查id不重複

2.name須符合英文名字格式(只包含英文大小寫、每個單字開頭大寫)

3.currency需為USD或NTD，為USD時會轉為NTD並將price乘以31

4.price檢查不可超過2000 NTD
# 路徑(.\newproject\OrderApi\)
docker image：https://hub.docker.com/r/youthtion/img_orders/tags

cmd：docker-compose up

GET / POST：http://localhost/api/orders

TEST Page：http://localhost/swagger/index.html
#
cmd (dotnet)：dotnet run

GET / POST (dotnet)：http://localhost:5210/api/orders

TEST Page：http://localhost:5210/swagger/index.html
# 單元測試(.\newproject\UnitTest\OrderApi.Tests\)
cmd (dotnet)：dotnet test
