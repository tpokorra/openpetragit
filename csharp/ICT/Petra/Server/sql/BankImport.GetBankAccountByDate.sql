SELECT DISTINCT PUB_p_banking_details.*, PUB_p_partner_banking_details.p_partner_key_n AS PartnerKey
FROM PUB_a_gift, PUB_a_gift_batch, PUB_p_banking_details, PUB_p_partner_banking_details
--, PUB_p_banking_details_usage
WHERE PUB_a_gift_batch.a_ledger_number_i = PUB_a_gift.a_ledger_number_i
AND PUB_a_gift_batch.a_batch_number_i = PUB_a_gift.a_batch_number_i
AND PUB_p_partner_banking_details.p_partner_key_n = PUB_a_gift.p_donor_key_n
AND PUB_p_partner_banking_details.p_banking_details_key_i = PUB_p_banking_details.p_banking_details_key_i
--AND PUB_p_banking_details_usage.p_banking_details_key_i = PUB_p_banking_details.p_banking_details_key_i
--AND PUB_p_banking_details_usage.p_partner_key_n = PUB_p_partner_banking_details.p_partner_key_n
--AND ((PUB_p_banking_details_usage.p_type_c = "MAIN" AND 
--    NOT EXISTS(SELECT * 
--               FROM PUB_p_banking_details_usage 
--               WHERE PUB_p_banking_details_usage.p_banking_details_key_i = PUB_p_banking_details.p_banking_details_key_i
--               AND PUB_p_banking_details_usage.p_partner_key_n = PUB_p_partner_banking_details.p_partner_key_n
--               AND PUB_p_banking_details_usage.p_type_c = "DONATIONS"))
--     OR
--     PUB_p_banking_details_usage.p_type_c = "DONATIONS"
--     OR
--        NOT EXISTS(SELECT * 
--               FROM PUB_p_banking_details_usage
--               WHERE PUB_p_banking_details_usage.p_banking_details_key_i = PUB_p_banking_details.p_banking_details_key_i
--               AND PUB_p_banking_details_usage.p_partner_key_n = PUB_p_partner_banking_details.p_partner_key_n
--               AND PUB_p_banking_details_usage.p_type_c = "MAIN")
--     )
AND PUB_a_gift_batch.a_gl_effective_date_d = ?
