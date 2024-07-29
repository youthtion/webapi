# WEBAPI(.\newproject\OrderApi\)
docker image：https://hub.docker.com/r/youthtion/img_orders/tags

cmd：docker-compose up

GET / POST：http://localhost/api/orders
#
cmd (dotnet)：dotnet run

GET / POST (dotnet)：http://localhost:5210/api/orders
# 單元測試(.\newproject\UnitTest\OrderApi.Tests\)
cmd (dotnet)：dotnet test
# SOLID原則
SRP：OrderData為主要資料型態和存放。

OCP：IName宣告檢查名字格式，讓子類(目前只有EnglishName)實作其他語言的檢查邏輯。考慮到各語言邏輯差異大，用此原則擴充。

DIP：讓OrderManager.IName透過其他實作IName的類別(目前只有EnglishName)執行。
# 資料庫
SELECT O.bnb_id, B.name AS bnb_name, SUM(O.amount) AS may_amount

FROM orders AS O

INNER JOIN bnbs AS B ON (O.created_at BETWEEN '2023-05-01 00:00:00' AND '2023-05-31 23:59:59') AND O.currency = 'TWD' AND B.id = O.bnb_id

GROUP BY O.bnb_id, B.name

ORDER BY may_amount DESC

LIMIT 10
#
比較常用MSSQL的Console下查詢，效能差的話可以直接看執行時間。

先減少在Where用邏輯計算式、有Join的話把可搬的條件放到On讓數量大的表先過濾資料。
