docker run -p 6001:80 -e EndpointUri="https://REPLACE_WITH_ACCOUNT_NAME.documents.azure.com:443/" -e PrimaryKey="REPLACE_WITH_YOUR_KEY" -e DatabaseId="TodoSaaS" -e ContainerId="Items" todo-service:latest