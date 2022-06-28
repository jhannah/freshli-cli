Feature: Compute libyear
  Scenario: Compute
    Given a file named "cyclonedx.json" with:
    """
    {
      "bomFormat": "CycloneDX",
      "specVersion": "1.4",
      "serialNumber": "urn:uuid:3e671687-395b-41f5-a30f-a58921a69b79",
      "version": 1,
      "components": [
        {
          "type": "library",
          "name": "calculatron",
          "version": "14.6",
          "purl": "pkg:nuget/org.corgibytes.calculatron/calculatron@14.6"
        },
        {
          "type": "library",
          "name": "flyswatter",
          "version": "1.1.0",
          "purl": "pkg:nuget/org.corgibytes.flyswatter/flyswatter@1.1.0"
        },
        {
          "type": "library",
          "name": "auto-cup-of-tea",
          "version": "112.0",
          "purl": "pkg:composer/org.corgibytes.tea/auto-cup-of-tea@112.0"
        }
      ]
    }
    """
    When I run `freshli compute-libyear cyclonedx.json`
    Then the output should contain:
    """
    calculatron                    14.6  12/31/2019             21.3  10/16/2022     2.8
    """
    Then the output should contain:
    """
    flyswatter                    1.1.0  11/29/1990            1.1.0  11/29/1990       0
    """
    Then the output should contain:
    """
    auto-cup-of-tea       Latest version could not be found in list for this package url
    """
    Then the output should contain:
    """
                                                                           Total     2.8
    """

