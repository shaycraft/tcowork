select max(s.id) as id, max(s.unit_size) as unit_size, s.formation, s.lkey, st_union(q.geom) as geom from staging.spacing s inner join plss.qq q on q.secdivid=s.lkey group by s.formation, s.lkey;
