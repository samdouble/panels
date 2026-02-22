# Changelog

## [1.7.0](https://github.com/samdouble/panels/compare/v1.6.0...v1.7.0) (2026-02-22)


### Features

* add page numbers at the bottom of the pages ([98dcd11](https://github.com/samdouble/panels/commit/98dcd11eecd4e8b8774c9032690ae7940c22a615))

## [1.6.0](https://github.com/samdouble/panels/compare/v1.5.0...v1.6.0) (2026-02-21)


### Features

* add border: none option on panels ([0f0bffd](https://github.com/samdouble/panels/commit/0f0bffd41afd5d25ea369fdf90aeaed8f3184376))
* add support for YAML configuration files ([9226623](https://github.com/samdouble/panels/commit/9226623c7aec8f2dea67a7a1ce623ee397db4ea4))

## [1.5.0](https://github.com/samdouble/panels/compare/v1.3.1...v1.5.0) (2026-02-21)


### Features

* add cropBottom and CropTop option on panels ([492a821](https://github.com/samdouble/panels/commit/492a8211debbf80de4a381162924c26bc6485bdb))
* add XSD validation ([96f4e27](https://github.com/samdouble/panels/commit/96f4e278c18c66302b9dfba25ac5effde404cece))
* added character field to text tag ([5cc5ef4](https://github.com/samdouble/panels/commit/5cc5ef4a6b6e927822c4e0a5e957fe7e5a76f1dc))
* be able to control font size through XML configuration ([fdedd51](https://github.com/samdouble/panels/commit/fdedd51699e4b2b8ff4e58f46b9d185b4e57cf44))
* cease support for .NET 9 ([1a2fb59](https://github.com/samdouble/panels/commit/1a2fb59b772610c57552926118fd1edb2b8d4b2d))
* rename crop by padding ([99100cf](https://github.com/samdouble/panels/commit/99100cfb5bad03179e820702ed529fae4cffb8ef))
* rename margin attributes ([841c603](https://github.com/samdouble/panels/commit/841c6035ded98c2ac6826165ebf23e02b1165230))
* split command-line parser into generate and validate commands ([cc68b4e](https://github.com/samdouble/panels/commit/cc68b4e96066a44b770016bd96186dc9c996c78b))
* support JSON configurations ([765a365](https://github.com/samdouble/panels/commit/765a36517384363e75c309efcdf22e59fba4f73a))


### Bug Fixes

* add width property to XSD for text element ([bc56dae](https://github.com/samdouble/panels/commit/bc56daeec745125a751098181a9c2aa97d452017))
* ci ([820f878](https://github.com/samdouble/panels/commit/820f8785b6c8b329877b6731ce7759df36f9ad12))
* ci ([76d2a5f](https://github.com/samdouble/panels/commit/76d2a5fed2461b404ee953106eef10d6bf77c63f))
* conflicts with master ([#6](https://github.com/samdouble/panels/issues/6)) ([9f5f001](https://github.com/samdouble/panels/commit/9f5f00136d43d668616f6dc5cfc262359d7ea27a))
* do not trim unused code during publish ([0500cad](https://github.com/samdouble/panels/commit/0500cad4b6d3c18ab1c0b026f733cde23fbbeb60))
* fix issue with description texts not showing ([eac1269](https://github.com/samdouble/panels/commit/eac12698857b948f098791f8234376fe44eef039))
* fix issue with fontSize propagation to children ([4dfea10](https://github.com/samdouble/panels/commit/4dfea108622ebf1706acfd6fbaa0f5fa8da06a10))
* fixed bug with new pages created when they should not ([#19](https://github.com/samdouble/panels/issues/19)) ([709e874](https://github.com/samdouble/panels/commit/709e8743d8abe1ab1f0e5e9a5d18fe6bfa5e6809))
* fixed crash with CLI arguments on .NET 7 ([#13](https://github.com/samdouble/panels/issues/13)) ([16839c0](https://github.com/samdouble/panels/commit/16839c0097e55d0a600bfabd17459aa85d31c60e))
* removed .NET 8 as target framework ([#20](https://github.com/samdouble/panels/issues/20)) ([ef1293d](https://github.com/samdouble/panels/commit/ef1293d0e16a00209f401392fd6b2bb150b2d69e))
* text width is now respected ([5c4d8d2](https://github.com/samdouble/panels/commit/5c4d8d2f2e26af0b088b341ffd80f9aa22d82055))


### Miscellaneous Chores

* release 1.4.6 ([08d3142](https://github.com/samdouble/panels/commit/08d314240fca7b84e4e1742da20864d2541494f0))
* release 1.5.0 ([4c1b362](https://github.com/samdouble/panels/commit/4c1b362f45e7807a639262672523a29623a36395))

## [1.4.6](https://github.com/samdouble/panels/compare/1.4.5...v1.4.6) (2025-10-16)


### Features

* rename crop by padding ([99100cf](https://github.com/samdouble/panels/commit/99100cfb5bad03179e820702ed529fae4cffb8ef))
* support JSON configurations ([765a365](https://github.com/samdouble/panels/commit/765a36517384363e75c309efcdf22e59fba4f73a))

### Bug Fixes

* make Comic a parameterless constructor ([f4cf6a5](https://github.com/samdouble/panels/commit/f4cf6a56c6e7d1a7748311b681e89fc27f35a1e0))
* fix issue with fontSize propagation to children ([4dfea10](https://github.com/samdouble/panels/commit/4dfea108622ebf1706acfd6fbaa0f5fa8da06a10))
* fix issue with description texts not showing ([eac1269](https://github.com/samdouble/panels/commit/eac12698857b948f098791f8234376fe44eef039))

## [1.3.0](https://github.com/samdouble/panels/compare/1.2.2...v1.3.0) (2025-07-27)


### Features

* add cropBottom and CropTop option on panels ([492a821](https://github.com/samdouble/panels/commit/492a8211debbf80de4a381162924c26bc6485bdb))
* split command-line parser into generate and validate commands ([cc68b4e](https://github.com/samdouble/panels/commit/cc68b4e96066a44b770016bd96186dc9c996c78b))


### Bug Fixes

* add width property to XSD for text element ([bc56dae](https://github.com/samdouble/panels/commit/bc56daeec745125a751098181a9c2aa97d452017))

## [1.2.2](https://github.com/samdouble/panels/compare/v1.2.1...v1.2.2) (2025-07-23)


### Bug Fixes

* ci ([820f878](https://github.com/samdouble/panels/commit/820f8785b6c8b329877b6731ce7759df36f9ad12))

## [1.2.1](https://github.com/samdouble/panels/compare/v1.2.0...v1.2.1) (2025-07-23)


### Bug Fixes

* ci ([76d2a5f](https://github.com/samdouble/panels/commit/76d2a5fed2461b404ee953106eef10d6bf77c63f))

## [1.2.0](https://github.com/samdouble/panels/compare/1.1.5...v1.2.0) (2025-07-23)


### Features

* add XSD validation ([96f4e27](https://github.com/samdouble/panels/commit/96f4e278c18c66302b9dfba25ac5effde404cece))
* be able to control font size through XML configuration ([fdedd51](https://github.com/samdouble/panels/commit/fdedd51699e4b2b8ff4e58f46b9d185b4e57cf44))
* rename margin attributes ([841c603](https://github.com/samdouble/panels/commit/841c6035ded98c2ac6826165ebf23e02b1165230))

## [1.1.11](https://github.com/samdouble/panels/compare/v1.1.10...v1.1.11) (2025-07-15)


### Bug Fixes

* fix CI ([3dafb99](https://github.com/samdouble/panels/commit/3dafb99d799fdc31f968a5db115a3843a9770e7a))

## [1.1.10](https://github.com/samdouble/panels/compare/1.1.9...v1.1.10) (2025-07-15)


### Bug Fixes

* fix CI ([dabb21a](https://github.com/samdouble/panels/commit/dabb21ac0759e4664a21047a04560f8f3df542d0))

## [1.1.8](https://github.com/samdouble/panels/compare/v1.1.7...v1.1.8) (2025-07-13)


### Bug Fixes

* fix CI ([b435f16](https://github.com/samdouble/panels/commit/b435f166e6c8a8648a6792473e79247c2c39c2d6))

## [1.1.7](https://github.com/samdouble/panels/compare/1.1.6...v1.1.7) (2025-07-13)


### Bug Fixes

* fix CI ([2187fa2](https://github.com/samdouble/panels/commit/2187fa266bc6238820458a007c9b7babf0d60b6c))
* fix CI ([05807a3](https://github.com/samdouble/panels/commit/05807a37b7d2ccae481d5c40b6f6229f18c3ac76))
* fix deployment workflow ([aeac5ac](https://github.com/samdouble/panels/commit/aeac5ac61e78e37d3be8f13065f5ad62ebfd878a))
* fix deployment workflow ([5bf06d2](https://github.com/samdouble/panels/commit/5bf06d2097c144b35ab09ea02ce81e83c68c8a62))
* fix deployment workflow ([e447f87](https://github.com/samdouble/panels/commit/e447f879867f4f6ac6943ae88b7df1cae7e93f82))
* fix deployment workflow ([e297c40](https://github.com/samdouble/panels/commit/e297c4079373757427adccb675811f149bbeaaaa))
* fix deployment workflow ([e72e8e9](https://github.com/samdouble/panels/commit/e72e8e9ab1257bf19f3b6d7b65c27694c127eb31))
* fix deployment workflow ([bec5f88](https://github.com/samdouble/panels/commit/bec5f880af29585c720a35e7bd2884ea7fa9a656))
* fix deployment workflow ([e6adac8](https://github.com/samdouble/panels/commit/e6adac8bf5fc93bcb3bdf3f225930aabdfef4154))
* fix deployment workflow ([aa460e1](https://github.com/samdouble/panels/commit/aa460e12aab2d0a23ee848d7ebac06029b887a72))
* fix deployment workflow ([71a1785](https://github.com/samdouble/panels/commit/71a1785a3d1e6dea3fb9494c0eff1b630683a108))
* fix deployment workflow ([da84839](https://github.com/samdouble/panels/commit/da84839575092d7f66d6559c1e8b38a6ce09f220))
* fix deployment workflow ([269764a](https://github.com/samdouble/panels/commit/269764ab51048c1197ecb9e4ef84101780c71c4c))
* fix deployment workflow ([da39642](https://github.com/samdouble/panels/commit/da39642b1071554d566f9cbcc98ddc8a125b7dbf))
* fix deployment workflow ([63dbcbd](https://github.com/samdouble/panels/commit/63dbcbdd14906bd3520e9f7b65c53f46e86f3e70))
* fix deployment workflow ([d17b1c9](https://github.com/samdouble/panels/commit/d17b1c9a82d8d6c2b5c1dfe58e9c56a504c6909b))
* fix deployment workflow ([6a6003c](https://github.com/samdouble/panels/commit/6a6003cc1a98d3cf60ea9c5048819b248f8970bf))
* fix deployment workflow ([77fb450](https://github.com/samdouble/panels/commit/77fb450788ad7c520b85a8d1f5937396aef0dbd2))
* fix deployment workflow ([5c46cd5](https://github.com/samdouble/panels/commit/5c46cd510a78dfa6bee7687264770467bff496eb))
* fix deployment workflow ([790b971](https://github.com/samdouble/panels/commit/790b971de564ed0434033397d6f51bbd2baabc05))
* fix deployment workflow ([61dfa19](https://github.com/samdouble/panels/commit/61dfa1938a8120c3c87b17a6c4be0f8feb0d32ec))
* fix deployment workflow ([0d58516](https://github.com/samdouble/panels/commit/0d58516e0715f2d3f582f5c5368db00d106987a4))
* fix deployment workflow ([9b12a95](https://github.com/samdouble/panels/commit/9b12a95c1d9bac58570d425a57174064d49a52ba))
* fix deployment workflow ([868833d](https://github.com/samdouble/panels/commit/868833d537c290c075d7de9c8d1346f88eced01e))
