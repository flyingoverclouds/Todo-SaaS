$ACRNAME="todosaas"
$ImgName="todo-portal-front"
$ContainerAppRG="TodoSaaS-portal"
$ContainerAppName="portaltds"
$BUILDTAG=Get-date -Format FileDateTime
$IMGTAG=$ImgName + ":" + $BUILDTAG
$fullImgName=$ACRNAME + ".azurecr.io/" + $IMGTAG

Write-Host Target ACR : $ACRNAME
Write-Host Build tag : $BUILDTAG

az acr login --name $ACRNAME

Write-Host "** ACR building image : " $IMGTAG
az acr build --registry $ACRNAME -t $IMGTAG -f portal-front/Dockerfile .

Write-Host "** ACR Available Images for : " $ImgName 
az acr repository show-tags --name $ACRNAME  --repository $ImgName -o table


Write-Host "** Updating container app with : " $fullImgName
az containerapp update --name $ContainerAppName --resource-group $ContainerAppRG --image $fullImgName -o table

