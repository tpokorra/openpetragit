-- passwords are inserted by OpenPetra.build
INSERT INTO s_user(s_user_id_c, s_password_hash_c, s_password_salt_c, s_password_needs_change_l) VALUES('DEMO', '{#PASSWORDHASHDEMO}', '{#PASSWORDSALTDEMO}', true);
INSERT INTO s_user(s_user_id_c, s_password_hash_c, s_password_salt_c, s_password_needs_change_l) VALUES('ANONYMOUS', '{#PASSWORDHASHANONYMOUS}', '{#PASSWORDSALTANONYMOUS}', false);

INSERT INTO s_module(s_module_id_c, s_module_name_c) VALUES('LEDGER0043', 'LEDGER0043');

-- setup the sample user DEMO
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('DEMO', 'PTNRUSER', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('DEMO', 'CONFERENCE', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('DEMO', 'DEVUSER', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('DEMO', 'PERSONNEL', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('DEMO', 'FINANCE-1', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('DEMO', 'FINANCE-2', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('DEMO', 'FINANCE-3', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('DEMO', 'LEDGER0043', true);
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_partner');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_partner_location');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_partner_type');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_location');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_church');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_family');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_person');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_unit');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_bank');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_venue');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_organisation');

-- setup the user ANONYMOUS for the webforms
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('ANONYMOUS', 'PTNRUSER', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('ANONYMOUS', 'CONFERENCE', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('ANONYMOUS', 'DEVUSER', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('ANONYMOUS', 'PERSONNEL', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('ANONYMOUS', 'FINANCE-1', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('ANONYMOUS', 'FINANCE-2', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('ANONYMOUS', 'FINANCE-3', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('ANONYMOUS', 'LEDGER0043', true);
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('ANONYMOUS', 'p_partner');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('ANONYMOUS', 'p_partner_location');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('ANONYMOUS', 'p_partner_type');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('ANONYMOUS', 'p_location');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('ANONYMOUS', 'p_church');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('ANONYMOUS', 'p_family');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('ANONYMOUS', 'p_person');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('ANONYMOUS', 'p_unit');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('ANONYMOUS', 'p_bank');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('ANONYMOUS', 'p_venue');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('ANONYMOUS', 'p_organisation');

-- setup the sample site Germany 43000000
INSERT INTO s_system_defaults(s_default_code_c, s_default_description_c, s_default_value_c) VALUES ('CurrentDatabaseVersion', 'the currently installed release number, set by installer/patchtool', '{#RELEASEVERSION}');
INSERT INTO s_system_defaults(s_default_code_c, s_default_description_c, s_default_value_c) VALUES ('SiteKey', 'there has to be one site key for the database', '43000000');

INSERT INTO p_partner(p_partner_key_n,p_partner_short_name_c,p_partner_class_c,p_status_code_c) VALUES(43000000, 'Germany', 'UNIT', 'ACTIVE'); 
INSERT INTO p_partner_type(p_partner_key_n, p_type_code_c) VALUES(43000000, 'LEDGER'); 
INSERT INTO p_unit(p_partner_key_n,p_unit_name_c,u_unit_type_code_c) VALUES(43000000,'Germany','F'); 
INSERT INTO p_location(p_site_key_n, p_location_key_i, p_street_name_c, p_country_code_c) VALUES(43000000, 0, 'No valid address on file', '99');
INSERT INTO p_partner_ledger(p_partner_key_n, p_last_partner_id_i) VALUES(43000000, 5000); 
INSERT INTO p_partner_location(p_partner_key_n, p_site_key_n, p_location_key_i) VALUES(43000000, 43000000, 0);

INSERT INTO p_partner(p_partner_key_n,p_partner_short_name_c,p_partner_class_c,p_status_code_c) VALUES(1000000, 'The Organisation', 'UNIT', 'ACTIVE'); 
INSERT INTO p_unit(p_partner_key_n,p_unit_name_c,u_unit_type_code_c) VALUES(1000000,'The Organisation','R'); 
INSERT INTO um_unit_structure(um_parent_unit_key_n,um_child_unit_key_n) VALUES(1000000,1000000);

INSERT INTO um_unit_structure(um_parent_unit_key_n,um_child_unit_key_n) VALUES(1000000,43000000);

COPY um_job FROM '{#ABSOLUTEBASEDATAPATH}/um_job.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';

-- setup special funds
INSERT INTO p_partner(p_partner_key_n,p_partner_short_name_c,p_partner_class_c,p_status_code_c) VALUES(4000000, 'International Clearing House', 'UNIT', 'ACTIVE'); 
INSERT INTO p_partner_type(p_partner_key_n, p_type_code_c) VALUES(4000000, 'LEDGER'); 
INSERT INTO p_unit(p_partner_key_n,p_unit_name_c,u_unit_type_code_c) VALUES(4000000,'International Clearing House','D');
INSERT INTO p_partner_location(p_partner_key_n, p_site_key_n, p_location_key_i) VALUES(4000000, 0, 0);
INSERT INTO um_unit_structure(um_parent_unit_key_n,um_child_unit_key_n) VALUES(1000000,4000000);

INSERT INTO p_partner(p_partner_key_n,p_partner_short_name_c,p_partner_class_c,p_status_code_c) VALUES(95000000, 'Global Impact Fund', 'UNIT', 'ACTIVE'); 
INSERT INTO p_partner_type(p_partner_key_n, p_type_code_c) VALUES(95000000, 'LEDGER'); 
INSERT INTO p_unit(p_partner_key_n,p_unit_name_c,u_unit_type_code_c) VALUES(95000000,'Global Impact Fund','D'); 
INSERT INTO p_partner_location(p_partner_key_n, p_site_key_n, p_location_key_i) VALUES(95000000, 0, 0);
INSERT INTO um_unit_structure(um_parent_unit_key_n,um_child_unit_key_n) VALUES(1000000,95000000);

-- setup foreign ledgers
INSERT INTO p_partner(p_partner_key_n,p_partner_short_name_c,p_partner_class_c,p_status_code_c) VALUES(35000000, 'Switzerland', 'UNIT', 'ACTIVE'); 
INSERT INTO p_partner_type(p_partner_key_n, p_type_code_c) VALUES(35000000, 'LEDGER'); 
INSERT INTO p_unit(p_partner_key_n,p_unit_name_c,u_unit_type_code_c) VALUES(35000000,'Switzerland','F'); 
INSERT INTO p_partner_location(p_partner_key_n, p_site_key_n, p_location_key_i) VALUES(35000000, 0, 0);
INSERT INTO um_unit_structure(um_parent_unit_key_n,um_child_unit_key_n) VALUES(1000000,35000000);

INSERT INTO p_partner(p_partner_key_n,p_partner_short_name_c,p_partner_class_c,p_status_code_c) VALUES(73000000, 'Kenya', 'UNIT', 'ACTIVE'); 
INSERT INTO p_partner_type(p_partner_key_n, p_type_code_c) VALUES(73000000, 'LEDGER'); 
INSERT INTO p_unit(p_partner_key_n,p_unit_name_c,u_unit_type_code_c) VALUES(73000000,'Kenya','F'); 
INSERT INTO p_partner_location(p_partner_key_n, p_site_key_n, p_location_key_i) VALUES(73000000, 0, 0);
INSERT INTO um_unit_structure(um_parent_unit_key_n,um_child_unit_key_n) VALUES(1000000,73000000);

-- setup sample ledger
COPY a_ledger FROM '{#ABSOLUTEBASEDATAPATH}/a_ledger.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_cost_centre_types FROM '{#ABSOLUTEBASEDATAPATH}/a_cost_centre_types.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_cost_centre FROM '{#ABSOLUTEBASEDATAPATH}/a_cost_centre.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_account FROM '{#ABSOLUTEBASEDATAPATH}/a_account.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_account_property_code FROM '{#ABSOLUTEBASEDATAPATH}/a_account_property_code.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_account_property FROM '{#ABSOLUTEBASEDATAPATH}/a_account_property.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_account_hierarchy FROM '{#ABSOLUTEBASEDATAPATH}/a_account_hierarchy.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_account_hierarchy_detail FROM '{#ABSOLUTEBASEDATAPATH}/a_account_hierarchy_detail.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_accounting_period FROM '{#ABSOLUTEBASEDATAPATH}/a_accounting_period.csv.local' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_accounting_system_parameter FROM '{#ABSOLUTEBASEDATAPATH}/a_accounting_system_parameter.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_system_interface FROM '{#ABSOLUTEBASEDATAPATH}/a_system_interface.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_transaction_type FROM '{#ABSOLUTEBASEDATAPATH}/a_transaction_type.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_valid_ledger_number FROM '{#ABSOLUTEBASEDATAPATH}/a_valid_ledger_number.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_motivation_group FROM '{#ABSOLUTEBASEDATAPATH}/a_motivation_group.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_motivation_detail FROM '{#ABSOLUTEBASEDATAPATH}/a_motivation_detail.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';

COPY p_publication FROM '{#ABSOLUTEBASEDATAPATH}/p_publication.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
