


insert into hallo.dbo.Images(Id, Description, OrderNr)
SELECT b.BilledeID, b.Description, b.OrderNr 
from kobenhavn.kobenhavn.HK_BILLEDER b;

insert into hallo.dbo.Articles(
	Headline, 
	Author, 
	Date, 
	FrontpageText, 
	FrontpageImage_Id, 
	Text, 
	ApprovedByEditor, 
	IsPublic, 
	ArticleType, 
	Category_Id, 
	Category2_Id)
select   
	overskrift, 
	forfatter, 
	Dato, 
	forsidetekst, 
	forsidebilledeid, 
	Tekst, 
	ischeckedbyjens, 
	Publicarticle, 
	case when ArticleType = 'INFO' then 1 else 0 end, 
	Category, 
	Category2
from kobenhavn.kobenhavn.HK_artikler
order by dato