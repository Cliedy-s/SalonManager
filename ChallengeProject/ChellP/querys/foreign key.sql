#ALERT문
/*
ALTER TABLE 예약 ADD FOREIGN KEY (서비스) REFERENCES 서비스 (이름);
ALTER TABLE 회원 ADD FOREIGN KEY (선호서비스) REFERENCES 서비스 (이름);
ALTER TABLE 매출 ADD FOREIGN KEY (휴대전화번호) REFERENCES 회원 (휴대전화번호);
ALTER TABLE 예약 ADD FOREIGN KEY (휴대전화번호) REFERENCES 회원 (휴대전화번호);
ALTER TABLE 예약 ADD FOREIGN KEY (서비스_이름) REFERENCES 서비스 (이름);
ALTER TABLE 회원 ADD FOREIGN KEY (등급) REFERENCES 등급 (등급);
*/
#alter table 매출 drop foreign key 매출_ibfk_1;
ALTER TABLE 매출 ADD FOREIGN KEY (휴대전화번호) REFERENCES 회원 (휴대전화번호);
#alter table 예약 drop foreign key 예약_fk;
ALTER TABLE 예약 ADD FOREIGN KEY (휴대전화번호) REFERENCES 회원 (휴대전화번호);
alter table 매출 drop foreign key 매출_ibfk_1;
ALTER TABLE 매출 ADD FOREIGN KEY (서비스_이름) REFERENCES 서비스 (이름);

#SELECT
select * from 회원;
#select * from 회원리스트 where 성별 = '여';
select * from 회원;
select * from 회원리스트;
select * from 회원리스트2;
select * from 시스템_사용자;
select * from 예약;
select * from 매출;
select * from 소모품;
select * from 서비스;

#DELETE
DELETE FROM 회원 WHERE _회원번호 IN ( 6, 8 );
DELETE FROM 회원 ;

#INSERT
INSERT INTO 회원 VALUES(null, '신회원리스트소연', 1, date_format(now(), '%Y-%m-%d'), '01063055237', 0, 0);
INSERT INTO 회원 VALUES(null, '이승완', 0, date_format(now(), '%Y-%m-%d'), '01012345678', 0, 0);
INSERT INTO 회원 VALUES(null, '이지연', 1, date_format(now(), '%Y-%m-%d'), '01000000001', 0, 0);
INSERT INTO 회원 VALUES(null, '박진이', 1, date_format(now(), '%Y-%m-%d'), '01000000002', 0, 0);
INSERT INTO 회원 VALUES(null, '신지혜', 1, date_format(now(), '%Y-%m-%d'), '01000000003', 0, 0);
INSERT INTO 시스템_사용자 VALUES('bhb0047', '신소연', 'tlsth9189', '과장');

INSERT INTO 매출 VALUES();

#AUTOINCREMENT 초기화
ALTER TABLE 회원 AUTO_INCREMENT=0;