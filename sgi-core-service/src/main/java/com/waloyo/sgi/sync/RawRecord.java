package com.waloyo.sgi.sync;

import lombok.Builder;
import lombok.Getter;
import lombok.ToString;

import java.time.OffsetDateTime;
import java.util.Map;

@Getter
@Builder
@ToString
public class RawRecord {
    private String sourceDb;
    private String sourceTable;
    private String primaryKey;
    private Map<String, Object> data;
    private OffsetDateTime modifiedAt;
}
