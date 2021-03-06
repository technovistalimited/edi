-- Actual Disbursement
SELECT T.*
FROM (
	SELECT F.FundSourceName AS ManagingDP, 
		P.Id, P.DPProjectNo, P.Title,
		F2.Acronym AS DP,
		D.DisbursementDate, COALESCE(D.DisbursementToDate,D.DisbursementDate) AS DisbursementToDate,
		DATEDIFF(MONTH, D.DisbursementDate, COALESCE(D.DisbursementToDate,D.DisbursementDate)) AS NoOfMonthRange,
		D.DisbursedAmount, C.Name AS Currency, D.DisbursedAmountInUSD,
		AC.Name AS AidCategory --, D.LoanNumber
	FROM tblProjectInfo P
	LEFT JOIN tblFundSource F ON P.FundSourceId = F.Id
	JOIN tblProjectFundingActualDisbursement D ON P.Id = D.ProjectId
	LEFT JOIN tblFundSource F2 ON D.FundSourceId = F2.Id
	LEFT JOIN tblAidCategory AC ON D.AidCategoryId = AC.Id
	LEFT JOIN tblCurrency C ON D.DisbursedCurrencyId = C.Id
) T WHERE 0=0
AND T.NoOfMonthRange > 3
ORDER BY T.ManagingDP, T.DPProjectNo, T.DisbursementDate

-- Plan Disbursement
SELECT T.*
FROM (
	SELECT F.FundSourceName AS ManagingDP, 
		P.Id, P.DPProjectNo, P.Title,
		F2.Acronym AS DP,
		D.PlannedDisbursementPeriodFromDate, COALESCE(D.PlannedDisbursementPeriodToDate,D.PlannedDisbursementPeriodFromDate) AS PlanDisbursementToDate,
		DATEDIFF(MONTH, D.PlannedDisbursementPeriodFromDate, COALESCE(D.PlannedDisbursementPeriodToDate,D.PlannedDisbursementPeriodFromDate))+1 AS NoOfDaysRange,
		D.PlannedDisburseAmount, C.Name AS Currency, D.PlannedDisburseAmountInUSD,
		AC.Name AS AidCategory, D.LoanNumber
	FROM tblProjectInfo P
	LEFT JOIN tblFundSource F ON P.FundSourceId = F.Id
	JOIN tblProjectFundingPlannedDisbursement D ON P.Id = D.ProjectId
	LEFT JOIN tblFundSource F2 ON D.FundSourceId = F2.Id
	LEFT JOIN tblAidCategory AC ON D.AidCategoryId = AC.Id
	LEFT JOIN tblCurrency C ON D.PlannedDisbursementCurrencyId = C.Id
) T WHERE 0=0
AND T.NoOfDaysRange > 6

/*
SELECT SUM(D.DisbursedAmountInUSD) / 1000000
FROM tblProjectFundingActualDisbursement D
WHERE 0=0 AND D.IsDisbursedTrustFund = 0
AND COALESCE(D.DisbursementToDate,D.DisbursementDate) BETWEEN '2008-07-01' AND '2009-06-30'
*/

/* FY wise Disbursement Tranasactions
--===================================
SELECT * FROM tblProjectFundingActualDisbursement D WHERE 0=0 AND D.IsDisbursedTrustFund = 0
AND COALESCE(D.DisbursementToDate,D.DisbursementDate) < '2007-07-01'

SELECT * FROM tblProjectFundingActualDisbursement D WHERE 0=0 AND D.IsDisbursedTrustFund = 0
AND COALESCE(D.DisbursementToDate,D.DisbursementDate) BETWEEN '2007-07-01' AND '2008-06-30'

SELECT * FROM tblProjectFundingActualDisbursement D WHERE 0=0 AND D.IsDisbursedTrustFund = 0
AND COALESCE(D.DisbursementToDate,D.DisbursementDate) BETWEEN '2008-07-01' AND '2009-06-30'

SELECT * FROM tblProjectFundingActualDisbursement D WHERE 0=0 AND D.IsDisbursedTrustFund = 0
AND COALESCE(D.DisbursementToDate,D.DisbursementDate) BETWEEN '2009-07-01' AND '2010-06-30'

SELECT * FROM tblProjectFundingActualDisbursement D WHERE 0=0 AND D.IsDisbursedTrustFund = 0
AND COALESCE(D.DisbursementToDate,D.DisbursementDate) BETWEEN '2010-07-01' AND '2011-06-30'

SELECT * FROM tblProjectFundingActualDisbursement D WHERE 0=0 AND D.IsDisbursedTrustFund = 0
AND COALESCE(D.DisbursementToDate,D.DisbursementDate) BETWEEN '2011-07-01' AND '2012-06-30'

SELECT * FROM tblProjectFundingActualDisbursement D WHERE 0=0 AND D.IsDisbursedTrustFund = 0
AND COALESCE(D.DisbursementToDate,D.DisbursementDate) BETWEEN '2011-07-01' AND '2012-06-30'
*/
