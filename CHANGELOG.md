# Changelog

## [1.8.1](https://github.com/samdouble/panels/compare/v1.8.0...v1.8.1) (2026-06-23)


### Bug Fixes

* replace showPages by showPageNumbers in XSD spec ([c5ca378](https://github.com/samdouble/panels/commit/c5ca3786b9df3b69809971eef0cf1ab1e23e3c90))

## [1.8.0](https://github.com/samdouble/panels/compare/v1.7.0...v1.8.0) (2026-02-23)


### Features

* add bordersColor property on comic and panel ([b6a3391](https://github.com/samdouble/panels/commit/b6a3391a0306087ab4555d1f87b4ba42d2d349ae))
* add bordersWidth property on comic and panel ([5659cc8](https://github.com/samdouble/panels/commit/5659cc82a0fbf9e10c8db69f2c7f2a452490e25a))

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

## [1.2.0](https://github.com/samdouble/panels/compare/1.1.5...v1.2.0) (2025-07-23)


### Features

* add XSD validation ([96f4e27](https://github.com/samdouble/panels/commit/96f4e278c18c66302b9dfba25ac5effde404cece))
* be able to control font size through XML configuration ([fdedd51](https://github.com/samdouble/panels/commit/fdedd51699e4b2b8ff4e58f46b9d185b4e57cf44))
* rename margin attributes ([841c603](https://github.com/samdouble/panels/commit/841c6035ded98c2ac6826165ebf23e02b1165230))
