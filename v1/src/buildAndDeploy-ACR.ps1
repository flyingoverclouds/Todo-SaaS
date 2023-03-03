$ACRNAME="todosaas"
#$BUILDTAG=Get-date -Format FileDateTime
$BUILDTAG="1.0.0"
$FrontRazorName="front-razor"
$FrontRazorImgName=$FrontRazorName + ":" + $BUILDTAG
$TodoServiceName="todo-service"
$TodoServiceImgName=$TodoServiceName + ":" + $BUILDTAG

Write-Host Target ACR : $ACRNAME
Write-Host Build tag : $BUILDTAG

az acr login --name $ACRNAME

Write-Host "** ACR building image : " $FrontRazorImgName
az acr build --registry $ACRNAME -t $FrontRazorImgName -f front-razor/Dockerfile .

Write-Host "** ACR building image : " $TodoServiceImgName
az acr build --registry $ACRNAME -t $TodoServiceImgName -f todo-service/Dockerfile .

Write-Host "** ACR Available Images for : " $FrontRazorImgName
az acr repository show-tags --name $ACRNAME  --repository $FrontRazorName -o table
Write-Host "** ACR Available Images for : " $TodoServiceImgName
az acr repository show-tags --name $ACRNAME  --repository $TodoServiceName -o table


#$FrontContainerImage=$ACRNAME + ".azurecr.io/" + $IMGTAG
#Write-Host "** Updating container app with : " $fullImgName
#az containerapp update --name $ContainerAppName --resource-group $ContainerAppRG --image $fullImgName -o table

