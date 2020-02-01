docker stop single-sign-on-admin-api 
docker rm single-sign-on-admin-api  
docker run \
	--name=single-sign-on-admin-api \
	-d --restart unless-stopped \
	-p 5001:80 \
	-e "ConnectionStrings__DefaultConnection"="Host=localhost;Database=SingleSignOn;Username=postgres;Password=password1;" \
	-e "Url__Authority"="http://localhost:5000" \
	-e "Url__CorsUrl"="http://localhost:4200" \
	-e "DatabaseType"="Postgres" \
	laredoza/single-sign-on-admin-api:latest